using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Application.Helpers;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Enums;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
using ShippingManagementSystem.Domain.UserTypes;
using ShippingManagementSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class GroupServices : IGroupServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public GroupServices(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }

        public async Task<PaginationResponse<GroupDTO>> GetAllGroupsAsync(GroupParams param)
        {
            try
            {
                var spec = new GroupSpecification(param);
                var groups = await _unitOfWork.Repository<Group>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<Group>().CountAsync(spec);
                
                var groupDTOs = new List<GroupDTO>();
                
                foreach (var group in groups)
                {
                    groupDTOs.Add(new GroupDTO
                    {
                        Id = group.Id,
                        Name = group.Name,
                        CreationDate = group.CreationDate.ToString("yyyy-MM-dd HH:mm"),
                    });
                }
                
                return new PaginationResponse<GroupDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    groupDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving groups", ex);
            }
        }

        public async Task<GroupDTO?> GetGroupByIdAsync(int id)
        {
            try
            {
                var spec = new GroupSpecification(id);
                var group = await _unitOfWork.Repository<Group>().GetBySpecAsync(spec);
                
                if (group == null)
                    return null;
                                
                return new GroupDTO
                {
                    Id = group.Id,
                    Name = group.Name,
                    CreationDate = group.CreationDate.ToString("yyyy-MM-dd HH:mm"),
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving group with id {id}", ex);
            }
        }

        public async Task<(bool IsSuccess, string Message)> CreateGroupAsync(CreateGroupDTO groupDTO)
        {
            try
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync();
                if (groupDTO.Permissions == null && groupDTO.Permissions.Count <= 0)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, "Permissions are required to create a group");
                }

                var group = new Group
                {
                    Name = groupDTO.Name,
                    CreationDate = DateTime.Now
                };

                await _unitOfWork.Repository<Group>().Add(group);
                await _unitOfWork.Save();
                var id = group.Id;
                foreach (var item in groupDTO.Permissions)
                {
                    foreach (var value in item.Values) 
                    { 
                        var groupMedule = new GroupMedule
                        {
                            GroupId = id,
                            MeduleId = item.Id,
                            Permission = (Permission)value
                        };
                        await _unitOfWork.Repository<GroupMedule>().Add(groupMedule);
                    }
                }

                await _unitOfWork.Save();

                await _unitOfWork.CommitAsync();
                return (true, $"Group '{group.Name}' created successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return (false, $"Error creating group: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateGroupAsync(int id, UpdateGroupDTO groupDTO)
        {
            try
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync();

                var group = await _unitOfWork.Repository<Group>().GetById(id);
                
                if (group == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return (false, $"Group with id {id} not found");
                }
                if (groupDTO.Permissions != null && groupDTO.Permissions.Count > 0)
                {
                    // Clear existing permissions
                    var existingPermissions = await _unitOfWork.Repository<GroupMedule>().GetAll();
                    await _unitOfWork.Repository<GroupMedule>().DeleteRange(existingPermissions.Where(g => g.GroupId == groupDTO.Id));

                    var Id = group.Id;

                    // Add new permissions
                    foreach (var item in groupDTO.Permissions)
                    {
                        foreach (var value in item.Values)
                        {
                            var groupMedule = new GroupMedule
                            {
                                GroupId = Id,
                                MeduleId = item.Id,
                                Permission = (Permission)value
                            };
                            await _unitOfWork.Repository<GroupMedule>().Add(groupMedule);
                        }
                    }
                }

                // Only update Name if it's not null or empty
                if (!string.IsNullOrEmpty(groupDTO.Name))
                {
                    group.Name = groupDTO.Name;
                }
                
                _unitOfWork.Repository<Group>().Update(group);
                await _unitOfWork.Save();
                
                return (true, $"Group '{group.Name}' updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating group: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteGroupAsync(int id)
        {
            try
            {
                var group = await _unitOfWork.Repository<Group>().GetById(id);
                
                if (group == null)
                    return (false, $"Group with id {id} not found");
                
                // Check if group has related GroupMedules
                if (group.GroupMedules != null && group.GroupMedules.Any())
                {
                    return (false, "Cannot delete group as it has associated modules. Please remove the modules first.");
                }
               
                _unitOfWork.Repository<Group>().Delete(group.Id);
                await _unitOfWork.Save();
                
                return (true, $"Group '{group.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting group: {ex.Message}");
            }
        }
    }
} 