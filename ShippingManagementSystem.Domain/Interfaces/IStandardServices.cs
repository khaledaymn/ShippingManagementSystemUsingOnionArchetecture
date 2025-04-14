using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IStandardServices
    {
        Task<List<StandardDTO>> GetAllStandardsAsync();
        Task<(bool IsSuccess, string Message)> CreateStandardAsync(CreateStandardDTO standardDTO);
        Task<(bool IsSuccess, string Message)> UpdateStandardAsync(int id, UpdateStandardDTO standardDTO);
    }
} 