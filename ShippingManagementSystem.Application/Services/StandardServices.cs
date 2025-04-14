using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Application.Services
{
    public class StandardServices : IStandardServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public StandardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<StandardDTO>> GetAllStandardsAsync()
        {
            try
            {
                // Get all non-deleted standards
                var standards = await _unitOfWork.Repository<Standard>().GetAll();
                
                return standards.Select(MapStandardToDTO).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving standards", ex);
            }
        }

        public async Task<(bool IsSuccess, string Message)> CreateStandardAsync(CreateStandardDTO standardDTO)
        {
            try
            {
                var standard = new Standard
                {
                    StandardWeight = standardDTO.StandardWeight,
                    VillagePrice = standardDTO.VillagePrice,
                    KGprice = standardDTO.KGprice,
                };
                
                _unitOfWork.Repository<Standard>().Add(standard);
                await _unitOfWork.Save();
                
                return (true, $"Standard created successfully with ID: {standard.Id}");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating standard: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateStandardAsync(int id, UpdateStandardDTO standardDTO)
        {
            try
            {
                var standard = await _unitOfWork.Repository<Standard>().GetById(id);
                
                if (standard == null )
                    return (false, $"Standard with id {id} not found");
                
                // Only update properties that are provided (not null)
                if (standardDTO.StandardWeight.HasValue)
                {
                    standard.StandardWeight = standardDTO.StandardWeight.Value;
                }
                
                if (standardDTO.VillagePrice.HasValue)
                {
                    standard.VillagePrice = standardDTO.VillagePrice.Value;
                }
                
                if (standardDTO.KGprice.HasValue)
                {
                    standard.KGprice = standardDTO.KGprice.Value;
                }
                
                _unitOfWork.Repository<Standard>().Update(standard);
                await _unitOfWork.Save();
                
                return (true, $"Standard updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating standard: {ex.Message}");
            }
        }

        private StandardDTO MapStandardToDTO(Standard standard)
        {
            return new StandardDTO
            {
                Id = standard.Id,
                StandardWeight = standard.StandardWeight,
                VillagePrice = standard.VillagePrice,
                KGprice = standard.KGprice,
            };
        }
    }
} 