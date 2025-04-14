using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IBranchServices
    {
        Task<PaginationResponse<BranchDTO>> GetAllBranchesAsync(BranchParams param);
        Task<BranchDTO?> GetBranchByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateBranchAsync(CreateBranchDTO branchDTO);
        Task<(bool IsSuccess, string Message)> UpdateBranchAsync(int id, BranchDTO branchDTO);
        Task<(bool IsSuccess, string Message)> DeleteBranchAsync(int id);
    }
} 