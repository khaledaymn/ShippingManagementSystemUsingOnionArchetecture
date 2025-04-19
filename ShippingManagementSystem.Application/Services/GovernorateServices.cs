using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class GovernorateServices : IGovernorateServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public GovernorateServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<GovernorateDTO>> GetAllGovernoratesAsync(GovernorateParams param)
        {
            try
            {
                var spec = new GovernorateSpecification(param);
                var governorates = await _unitOfWork.Repository<Governorate>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<Governorate>().CountAsync(spec);
                
                var governorateDTOs = governorates.Select(g => new GovernorateDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsDeleted = g.IsDeleted
                }).ToList();
                
                return new PaginationResponse<GovernorateDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    governorateDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving governorates", ex);
            }
        }

        public async Task<GovernorateDTO?> GetGovernorateByIdAsync(int id)
        {
            try
            {
                var spec = new GovernorateSpecification(id);
                var governorate = await _unitOfWork.Repository<Governorate>().GetBySpecAsync(spec);
                
                if (governorate == null)
                    return null;
                
                return new GovernorateDTO
                {
                    Id = governorate.Id,
                    Name = governorate.Name,
                    IsDeleted = governorate.IsDeleted
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving governorate with id {id}", ex);
            }
        }

        public async Task<(bool IsSuccess, string Message)> CreateGovernorateAsync(CreateGovernorateDTO governorateDTO)
        {
            try
            {
                var governorate = new Governorate
                {
                    Name = governorateDTO.Name,
                    IsDeleted = false
                };
                
                _unitOfWork.Repository<Governorate>().Add(governorate);
                await _unitOfWork.Save();
                
                return (true, $"Governorate '{governorate.Name}' created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating governorate: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateGovernorateAsync(int id, GovernorateDTO governorateDTO)
        {
            try
            {
                var governorate = await _unitOfWork.Repository<Governorate>().GetById(id);
                
                if (governorate == null)
                    return (false, $"Governorate with id {id} not found");
                
                // Only update Name if it's not null or empty
                if (!string.IsNullOrEmpty(governorateDTO.Name))
                {
                    governorate.Name = governorateDTO.Name;
                }
                
                _unitOfWork.Repository<Governorate>().Update(governorate);
                await _unitOfWork.Save();
                
                return (true, $"Governorate '{governorate.Name}' updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating governorate: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteGovernorateAsync(int id)
        {
            try
            {
                var governorate = await _unitOfWork.Repository<Governorate>().GetById(id);
                
                if (governorate == null)
                    return (false, $"Governorate with id {id} not found");
                
                // Soft delete
                governorate.IsDeleted = !governorate.IsDeleted;
                _unitOfWork.Repository<Governorate>().Update(governorate);
                
                // Hard delete if needed
                // _unitOfWork.Repository<Governorate>().Delete(governorate);
                
                await _unitOfWork.Save();
                
                return (true, $"Governorate '{governorate.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting governorate: {ex.Message}");
            }
        }
    }
} 