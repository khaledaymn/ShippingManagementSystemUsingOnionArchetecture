using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IMerchantServices
    {
        Task<(bool IsSuccess, string Message)> AddMerchantAsync(AddMerchantDTO dto);
        Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param);
        Task<MerchantDTO?> GetMerchantByIdAsync(string id);
        Task<(bool IsSuccess, string Message)> UpdateMerchantAsync(UpdateMerchantDTO dto);
        Task<(bool IsSuccess, string Message)> DeleteMerchantAsync(string id);
    }
}