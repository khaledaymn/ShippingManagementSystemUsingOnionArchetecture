using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IRejectedReasonServices
    {
        Task<PaginationResponse<RejectedReasonDTO>> GetAllRejectedReasonsAsync(RejectedReasonParams param);
        Task<RejectedReasonDTO?> GetRejectedReasonByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateRejectedReasonAsync(CreateRejectedReasonDTO rejectedReasonDTO);
        Task<(bool IsSuccess, string Message)> UpdateRejectedReasonAsync(int id, RejectedReasonDTO rejectedReasonDTO);
        Task<(bool IsSuccess, string Message)> DeleteRejectedReasonAsync(int id);
    }
} 