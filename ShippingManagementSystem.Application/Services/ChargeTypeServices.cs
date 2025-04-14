using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class ChargeTypeServices : IChargeTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChargeTypeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<ChargeTypeDTO>> GetAllChargeTypesAsync(ChargeTypeParams param)
        {
            try
            {
                var spec = new ChargeTypeSpecification(param);
                var chargeTypes = await _unitOfWork.Repository<ChargeType>().GetAllBySpecAsync(spec);
                var count = await _unitOfWork.Repository<ChargeType>().CountAsync(spec);
                
                var chargeTypeDTOs = chargeTypes.Select(ct => new ChargeTypeDTO
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    ExtraPrice = ct.ExtraPrice,
                    NumOfDay = ct.NumOfDay,
                    IsDeleted = ct.IsDeleted
                }).ToList();
                
                return new PaginationResponse<ChargeTypeDTO>(
                    param.PageSize,
                    param.PageIndex,
                    count,
                    chargeTypeDTOs
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving charge types", ex);
            }
        }

        public async Task<ChargeTypeDTO?> GetChargeTypeByIdAsync(int id)
        {
            try
            {
                var spec = new ChargeTypeSpecification(id);
                var chargeType = await _unitOfWork.Repository<ChargeType>().GetBySpecAsync(spec);
                
                if (chargeType == null)
                    return null;
                
                return new ChargeTypeDTO
                {
                    Id = chargeType.Id,
                    Name = chargeType.Name,
                    ExtraPrice = chargeType.ExtraPrice,
                    NumOfDay = chargeType.NumOfDay,
                    IsDeleted = chargeType.IsDeleted
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving charge type with id {id}", ex);
            }
        }

        public async Task<(bool IsSuccess, string Message)> CreateChargeTypeAsync(CreateChargeTypeDTO chargeTypeDTO)
        {
            try
            {
                var chargeType = new ChargeType
                {
                    Name = chargeTypeDTO.Name,
                    ExtraPrice = chargeTypeDTO.ExtraPrice,
                    NumOfDay = chargeTypeDTO.NumOfDay,
                    IsDeleted = false
                };
                
                _unitOfWork.Repository<ChargeType>().Add(chargeType);
                await _unitOfWork.Save();
                
                return (true, $"Charge type '{chargeType.Name}' created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating charge type: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateChargeTypeAsync(int id, ChargeTypeDTO chargeTypeDTO)
        {
            try
            {
                var chargeType = await _unitOfWork.Repository<ChargeType>().GetById(id);
                
                if (chargeType == null)
                    return (false, $"Charge type with id {id} not found");
                
                // Only update properties that are provided (not null/empty)
                if (!string.IsNullOrEmpty(chargeTypeDTO.Name))
                {
                    chargeType.Name = chargeTypeDTO.Name;
                }
                
                // For numeric values, we need a different approach
                // We'll assume a value of 0 or less means "don't update"
                if (chargeTypeDTO.ExtraPrice > 0)
                {
                    chargeType.ExtraPrice = chargeTypeDTO.ExtraPrice;
                }
                
                if (chargeTypeDTO.NumOfDay > 0)
                {
                    chargeType.NumOfDay = chargeTypeDTO.NumOfDay;
                }
                
                _unitOfWork.Repository<ChargeType>().Update(chargeType);
                await _unitOfWork.Save();
                
                return (true, $"Charge type '{chargeType.Name}' updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating charge type: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteChargeTypeAsync(int id)
        {
            try
            {
                var chargeType = await _unitOfWork.Repository<ChargeType>().GetById(id);
                
                if (chargeType == null)
                    return (false, $"Charge type with id {id} not found");
                
                // Soft delete
                chargeType.IsDeleted = true;
                _unitOfWork.Repository<ChargeType>().Update(chargeType);
                
                // Hard delete if needed
                // _unitOfWork.Repository<ChargeType>().Delete(chargeType);
                
                await _unitOfWork.Save();
                
                return (true, $"Charge type '{chargeType.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting charge type: {ex.Message}");
            }
        }
    }
} 