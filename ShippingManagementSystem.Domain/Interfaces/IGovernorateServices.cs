using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IGovernorateServices
    {
        Task<PaginationResponse<GovernorateDTO>> GetAllGovernoratesAsync(GovernorateParams param);
        Task<GovernorateDTO?> GetGovernorateByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateGovernorateAsync(CreateGovernorateDTO governorateDTO);
        Task<(bool IsSuccess, string Message)> UpdateGovernorateAsync(int id, GovernorateDTO governorateDTO);
        Task<(bool IsSuccess, string Message)> DeleteGovernorateAsync(int id);
    }
} 