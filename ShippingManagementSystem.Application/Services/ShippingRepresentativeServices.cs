using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class ShippingRepresentativeServices : IShippingRepresentativeServices
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShippingRepresentativeServices(IUnitOfWork unit, UserManager<ApplicationUser> userManager)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        #region Add ShippingRepresentative
        public async Task<(bool IsSuccess, string Message)> AddShippingRepresentativeAsync(AddShippingRepresentativeDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email))
                return (false, "Invalid shipping representative data.");

            var newUser = new ApplicationUser
            {
                Name = dto.Name,
                UserName = dto.Name.Replace(" ", ""),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsDeleted = false,
                HiringDate = DateTime.Now,
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

                    var shippingRep = new ShippigRepresentative
                    {
                        UserID = newUser.Id,
                        DiscountType = dto.DiscountType,
                        CompanyPersentage = dto.CompanyPercentage
                    };
                    await _unit.Repository<ShippigRepresentative>().Add(shippingRep);

                    if (dto.GovernorateIds != null && dto.GovernorateIds.Any())
                    {
                        foreach (var governorateId in dto.GovernorateIds)
                        {
                            var shippingRepGovernorate = new ShippingRepGovernorate
                            {
                                ShippingRepId = newUser.Id,
                                GovernorateId = governorateId
                            };
                            await _unit.Repository<ShippingRepGovernorate>().Add(shippingRepGovernorate);
                        }
                    }

                    await _unit.Save();
                    await _unit.CommitAsync();

                    return (true, "Shipping representative created successfully.");
                }
                catch (Exception ex)
                {
                    await _unit.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion


        #region Get All ShippingRepresentatives
        public async Task<PaginationResponse<ShippingRepresentativeDTO>> GetAllShippingRepresentativesAsync(ShippingRepresentativeParams param)
        {
            var spec = new ShippingRepresentativeSpecification(param);

            var shippingReps = await _unit.Repository<ShippigRepresentative>().GetAllBySpecAsync(spec);

            var totalCount = await _unit.Repository<ShippigRepresentative>().CountAsync(spec);

            var result = shippingReps.Select(sr => new ShippingRepresentativeDTO
            {
                Id = sr.UserID,
                Name = sr.User?.Name ?? string.Empty,
                Email = sr.User?.Email ?? string.Empty,
                PhoneNumber = sr.User?.PhoneNumber ?? string.Empty,
                DiscountType = sr.DiscountType,
                CompanyPercentage = sr.CompanyPersentage,
                HiringDate = sr.User?.HiringDate ?? DateTime.UtcNow,
                Governorates = sr.ShippingRepGovernorates?
                    .Select(g => g.Governorate?.Name ?? "Unknown")
                    .ToList() ?? new List<string>()
            }).ToList();

            return new PaginationResponse<ShippingRepresentativeDTO>(
                pageSize: param.PageSize,
                pageIndex: param.PageIndex,
                totalCount: totalCount,
                data: result.AsReadOnly()
            );
        }
        #endregion


        #region Get ShippingRepresentative By Id
        public async Task<ShippingRepresentativeDTO?> GetShippingRepresentativeByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var spec = new ShippingRepresentativeSpecification(id);
            var shippingRep = await _unit.Repository<ShippigRepresentative>().GetBySpecAsync(spec);

            if (shippingRep == null)
                return null;

            return new ShippingRepresentativeDTO
            {
                Id = shippingRep.UserID,
                Name = shippingRep.User.Name,
                Email = shippingRep.User.Email,
                PhoneNumber = shippingRep.User.PhoneNumber,
                DiscountType = shippingRep.DiscountType,
                CompanyPercentage = shippingRep.CompanyPersentage,
                HiringDate = shippingRep.User.HiringDate,
                Governorates = shippingRep.ShippingRepGovernorates?.Select(g => g.Governorate?.Name ?? "Unknown").ToList() ?? new List<string>()
            };
        }
        #endregion


        #region Update ShippingRepresentative
        public async Task<(bool IsSuccess, string Message)> UpdateShippingRepresentativeAsync(UpdateShippingRepresentativeDTO dto)
        {
            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    if (string.IsNullOrEmpty(dto.Id))
                        throw new ArgumentException("Invalid shipping representative ID.");

                    var spec = new ShippingRepresentativeSpecification(dto.Id);
                    var shippingRep = await _unit.Repository<ShippigRepresentative>().GetBySpecAsync(spec);
                    if (shippingRep == null)
                        throw new InvalidOperationException("Shipping representative not found.");

                    shippingRep.DiscountType = dto.DiscountType ?? shippingRep.DiscountType;
                    shippingRep.CompanyPersentage = dto.CompanyPercentage ?? shippingRep.CompanyPersentage;

                    var govSpec = new ShippingRepresentativeSpecification(dto.Id);
                    var existingGovernorates = await _unit.Repository<ShippigRepresentative>().GetAllBySpecAsync(govSpec);

                    if (dto.GovernorateIds != null)
                    {
                        foreach (var govId in dto.GovernorateIds)
                        {
                            var newGov = new ShippingRepGovernorate
                            {
                                ShippingRepId = dto.Id,
                                GovernorateId = govId
                            };
                            await _unit.Repository<ShippingRepGovernorate>().Add(newGov);
                        }
                    }

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Shipping representative updated successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion


        #region Delete ShippingRepresentative
        public async Task<(bool IsSuccess, string Message)> DeleteShippingRepresentativeAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return (false, "Invalid shipping representative ID.");

            var spec = new ShippingRepresentativeSpecification(id);
            var shippingRep = await _unit.Repository<ShippigRepresentative>().GetBySpecAsync(spec);

            if (shippingRep == null)
                return (false, "Shipping representative not found.");

            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    // Soft delete the user
                    var user = shippingRep.User;
                    user.IsDeleted = true;
                    var userUpdateResult = await _userManager.UpdateAsync(user);

                    if (!userUpdateResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        var errors = string.Join("; ", userUpdateResult.Errors.Select(e => e.Description));
                        return (false, $"Delete failed: {errors}");
                    }

                    await _unit.Save();
                    await transaction.CommitAsync();

                    return (true, "Shipping representative deleted successfully.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"An error occurred: {ex.Message}");
                }
            }
        }
        #endregion
    }
}