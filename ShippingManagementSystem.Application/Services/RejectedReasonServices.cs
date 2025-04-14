using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class RejectedReasonServices : IRejectedReasonServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public RejectedReasonServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<RejectedReasonDTO>> GetAllRejectedReasonsAsync(RejectedReasonParams param)
        {
            try
            {
                var spec = new RejectedReasonSpecification(param);
                var rejectedReasons = await _unitOfWork.Repository<RejectedReason>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<RejectedReason>().CountAsync(spec);
                
                var rejectedReasonDTOs = rejectedReasons.Select(rr => new RejectedReasonDTO
                {
                    Id = rr.Id,
                    Text = rr.Text,
                    IsDeleted = rr.IsDeleted
                }).ToList();
                
                return new PaginationResponse<RejectedReasonDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    rejectedReasonDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving rejected reasons", ex);
            }
        }

        public async Task<RejectedReasonDTO?> GetRejectedReasonByIdAsync(int id)
        {
            try
            {
                var spec = new RejectedReasonSpecification(id);
                var rejectedReason = await _unitOfWork.Repository<RejectedReason>().GetBySpecAsync(spec);
                
                if (rejectedReason == null)
                    return null;
                
                return new RejectedReasonDTO
                {
                    Id = rejectedReason.Id,
                    Text = rejectedReason.Text,
                    IsDeleted = rejectedReason.IsDeleted
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving rejected reason with id {id}", ex);
            }
        }

        public async Task<(bool IsSuccess, string Message)> CreateRejectedReasonAsync(CreateRejectedReasonDTO rejectedReasonDTO)
        {
            try
            {
                var rejectedReason = new RejectedReason
                {
                    Text = rejectedReasonDTO.Text,
                    IsDeleted = false
                };
                
                _unitOfWork.Repository<RejectedReason>().Add(rejectedReason);
                await _unitOfWork.Save();
                
                return (true, $"Rejected reason created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating rejected reason: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateRejectedReasonAsync(int id, RejectedReasonDTO rejectedReasonDTO)
        {
            try
            {
                var rejectedReason = await _unitOfWork.Repository<RejectedReason>().GetById(id);
                
                if (rejectedReason == null)
                    return (false, $"Rejected reason with id {id} not found");
                
                // Only update Text if it's not null or empty
                if (!string.IsNullOrEmpty(rejectedReasonDTO.Text))
                {
                    rejectedReason.Text = rejectedReasonDTO.Text;
                }
                
                _unitOfWork.Repository<RejectedReason>().Update(rejectedReason);
                await _unitOfWork.Save();
                
                return (true, $"Rejected reason updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating rejected reason: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteRejectedReasonAsync(int id)
        {
            try
            {
                var rejectedReason = await _unitOfWork.Repository<RejectedReason>().GetById(id);
                
                if (rejectedReason == null)
                    return (false, $"Rejected reason with id {id} not found");
                
                // Soft delete
                rejectedReason.IsDeleted = true;
                _unitOfWork.Repository<RejectedReason>().Update(rejectedReason);
                
                // Hard delete if needed
                // _unitOfWork.Repository<RejectedReason>().Delete(rejectedReason);
                
                await _unitOfWork.Save();
                
                return (true, $"Rejected reason deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting rejected reason: {ex.Message}");
            }
        }
    }
} 