using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Domain.Specifications;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.EmployeeDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Infrastructure.Data;
using System.Security.Claims;
using ShippingManagementSystem.Application.Helper;

namespace ShippingManagementSystem.Domain.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeServices(IUnitOfWork unit, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context;
        }

        #region Add Employee
        public async Task<(bool IsSuccess, string Message)> AddEmployeeAsync(AddEmployeeDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email))
                return (false, "Invalid employee data.");
            if(await _userManager.FindByEmailAsync(dto.Email) != null)
                return (false, "Email already exists.");

            var random = new Random();
            var randomNumber = random.Next(00,99);
            var newUser = new ApplicationUser
            {
                Name = dto.Name,
                UserName = _userManager.FindByNameAsync(dto.Name.Replace(" ","")).Result is not null ? dto.Name.Replace(" ", "")+randomNumber : dto.Name.Replace(" ",""),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsDeleted = false,
                HiringDate = DateTime.Now,
                Address = dto.Address,
            };

            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(newUser, dto.Password);
                    if (!result.Succeeded)
                    {
                        await _unit.RollbackAsync();
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        return (false, $"Create failed: {errors}");
                    }
                    await _userManager.AddToRoleAsync(newUser, Roles.Employee);
                    if (dto.GroupId == null)
                    {
                        await _unit.RollbackAsync();
                        return (false, "Group is required.");
                    }

                    var employee = new Employee
                    {
                        UserID = newUser.Id,
                        GroupId = dto.GroupId,
                    };
                    await _unit.Repository<Employee>().Add(employee);

                    if (dto.BranchIds == null || !dto.BranchIds.Any())
                    {
                        await _unit.RollbackAsync();
                        return (false, "Branches are required.");
                    }

                    foreach (var branchId in dto.BranchIds)
                    {
                        var userBranch = new UserBranches
                        {
                            BranchId = branchId,
                            UserId = newUser.Id
                        };
                        await _unit.Repository<UserBranches>().Add(userBranch);
                    }

                    var GroupMedule = _context.GroupMedules.Where(gm => gm.GroupId == dto.GroupId).ToList();
                    if (GroupMedule == null || !GroupMedule.Any())
                    {
                        await _unit.RollbackAsync();
                        return (false, "No permissions found for the selected group.");
                    }

                    var UserClaims = new List<Claim>();
                    foreach (var gm in GroupMedule)
                    {
                        var claim = new Claim(Constants.Permission,$"{Constants.Permission}.{gm.Medule.Name}.{ gm.Permission.ToString()}");
                        UserClaims.Add(claim);
                    }
                    var addClaimResult = await _userManager.AddClaimsAsync(newUser, UserClaims);

                    if (!addClaimResult.Succeeded)
                    {
                        await _unit.RollbackAsync();
                        var errors = string.Join(", ", addClaimResult.Errors.Select(e => e.Description));
                        return (false, $"Add claims failed: {errors}");
                    }

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Employee created successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }

        #endregion


        #region Get All 

        public async Task<PaginationResponse<EmployeeDTO>> GetAllEmployeesAsync(EmployeeParams param)
        {
            var spec = new EmployeeSpecification(param);
            var employees = await _unit.Repository<Employee>().GetAllBySpecAsync(spec);

            var totalCount = await _unit.Repository<Employee>().CountAsync(spec);

            var result = employees.Select(emp => new EmployeeDTO
            {
                Id = emp.UserID,
                Name = emp.User?.Name ?? string.Empty,
                Email = emp.User?.Email ?? string.Empty,
                PhoneNumber = emp.User?.PhoneNumber ?? string.Empty,
                CreationDate = emp.User?.HiringDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Branches = emp.User?.UserBranches?
                    .Select(ub => new GetBranchDTO
                    {
                        Id = ub.BranchId,
                        Name = ub.Branch?.Name ?? string.Empty,
                        CreationDate = ub.Branch?.CreationDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        IsDeleted = ub.Branch?.IsDeleted ?? false,
                        CityName = ub.Branch.City?.Name,
                        Location = ub.Branch?.Location
                    })
                    .ToList() ?? new List<GetBranchDTO>(),
                Permission = emp.Group.Name ?? string.Empty, // فعّلي لو فيه حقل Permission
                IsDeleted = emp.User?.IsDeleted ?? false,
                Address = emp.User?.Address ?? string.Empty
            }).ToList();

            return new PaginationResponse<EmployeeDTO>(
                pageSize: param.PageSize,
                pageIndex: param.PageIndex,
                totalCount: totalCount,
                data: result.AsReadOnly()
            );
        }

        #endregion


        #region Get Employee By Id
        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var spec = new EmployeeSpecification(p => p.UserID == id);
            var employee = await _unit.Repository<Employee>().GetBySpecAsync(spec);

            if (employee == null)
                return null;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            return new EmployeeDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreationDate = user.HiringDate.ToString("yyyy-MM-dd"),
                Branches = user.UserBranches?
                    .Select(ub => new GetBranchDTO
                    {
                        Id = ub.BranchId,
                        Name = ub.Branch?.Name ?? string.Empty,
                        CreationDate = ub.Branch?.CreationDate.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
                        IsDeleted = ub.Branch?.IsDeleted ?? false
                    })
                    .ToList() ?? new List<GetBranchDTO>(),
                Permission = employee.Group.Name ?? string.Empty,
                IsDeleted = user.IsDeleted,
                Address = user.Address ?? string.Empty
            };
        }

        #endregion


        #region Update Employee
        public async Task<(bool IsSuccess, string Message)> UpdateEmployeeAsync(UpdateEmployeeDTO dto)
        {
            using var transaction = await _unit.BeginTransactionAsync();
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.Id))
                    throw new ArgumentException("Invalid employee data.");

                var user = await _userManager.FindByIdAsync(dto.Id);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                var spec = new EmployeeSpecification(e => e.UserID == dto.Id);
                var employee = await _unit.Repository<Employee>().GetBySpecAsync(spec);
                if (employee == null)
                    throw new InvalidOperationException("Employee not found.");
                if(!string.IsNullOrEmpty(dto.Name))
                    user.Name = dto.Name;
                if (!string.IsNullOrEmpty(dto.Email))
                    user.Email = dto.Email;
                if (!string.IsNullOrEmpty(dto.PhoneNumber))
                    user.PhoneNumber = dto.PhoneNumber;
                if (!string.IsNullOrEmpty(dto.Address))
                    user.Address = dto.Address;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Update failed: {errors}");
                }

                if (dto.GroupId.HasValue)
                {
                    employee.GroupId = dto.GroupId.Value;
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    foreach (var claim in userClaims)
                    {
                        if (claim.Type == Constants.Permission)
                            await _userManager.RemoveClaimAsync(user, claim);
                    }
                    var GroupMedule = _context.GroupMedules.Where(gm => gm.GroupId == dto.GroupId).ToList();
                    var UserClaims = new List<Claim>();
                    foreach (var gm in GroupMedule)
                    {
                        var claim = new Claim(Constants.Permission, $"{Constants.Permission}.{gm.Medule.Name}.{gm.Permission.ToString()}");
                        UserClaims.Add(claim);
                    }
                    var addClaimResult = await _userManager.AddClaimsAsync(user, UserClaims);
                    if (!addClaimResult.Succeeded)
                    {
                        var errors = string.Join(", ", addClaimResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Add claims failed: {errors}");
                    }
                    _unit.Repository<Employee>().Update(employee);
                }

                if (dto.BranchIds != null && dto.BranchIds.Any())
                {
                    var existingBranches = _unit.Repository<UserBranches>()
                        .GetAll().Result
                        .Where(ub => ub.UserId == dto.Id)
                        .ToList();
                    
                    await _unit.Repository<UserBranches>().DeleteRange(existingBranches);

                    foreach (var branchId in dto.BranchIds)
                    {
                        var userBranch = new UserBranches
                        {
                            BranchId = branchId,
                            UserId = dto.Id
                        };
                        await _unit.Repository<UserBranches>().Add(userBranch);
                    }
                }

                await _unit.Save();
                await _unit.CommitAsync();

                return (true, "Employee updated successfully.");
            }
            catch (Exception ex)
            {
                await _unit.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}");
            }

        }

        #endregion


        #region Delete Employee

        public async Task<(bool IsSuccess, string Message)> DeleteEmployeeAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentException("Invalid employee ID.");

                var spec = new EmployeeSpecification(e => e.UserID == id);
                var employee = await _unit.Repository<Employee>().GetBySpecAsync(spec);
                if (employee == null)
                    throw new InvalidOperationException("Employee not found.");

                employee.User.IsDeleted = !employee.User.IsDeleted;
               
                _unit.Repository<Employee>().Update(employee);

                await _unit.Save();

                return (true, "Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        #endregion

    }
}
