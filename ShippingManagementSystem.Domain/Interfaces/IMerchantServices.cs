using Shipping_Project.DTOs.MerchantDtos;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IMerchantServices
    {
        Task<(bool IsSuccess, string Message)> AddMerchantAsync(MerchantDtoForAdding dto);
        Task<PaginationResponse<MerchantDTO>> GetAllMerchantsAsync(MerchantParams param);
        Task<MerchecntForEditingAndGetting?> GetMerchantByIdAsync(string id);
        Task<(bool IsSuccess, string Message)> UpdateMerchantAsync(MerchecntForEditingAndGetting dto);
        Task<(bool IsSuccess, string Message)> DeleteMerchantAsync(string id);
    }
}