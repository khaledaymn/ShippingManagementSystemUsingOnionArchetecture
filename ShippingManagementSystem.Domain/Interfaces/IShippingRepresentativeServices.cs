using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IShippingRepresentativeServices
    {
        Task<(bool IsSuccess, string Message)> AddShippingRepresentativeAsync(AddShippingRepresentativeDTO dto);
        Task<PaginationResponse<ShippingRepresentativeDTO>> GetAllShippingRepresentativesAsync(ShippingRepresentativeParams param);
        Task<ShippingRepresentativeDTO?> GetShippingRepresentativeByIdAsync(string id);
        Task<(bool IsSuccess, string Message)> UpdateShippingRepresentativeAsync(UpdateShippingRepresentativeDTO dto);
        Task<(bool IsSuccess, string Message)> DeleteShippingRepresentativeAsync(string id);
    }
}