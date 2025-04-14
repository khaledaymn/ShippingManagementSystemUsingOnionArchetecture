using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Interfaces
{
    public interface IGroupServices
    {
        Task<PaginationResponse<GroupDTO>> GetAllGroupsAsync(GroupParams param);
        Task<GroupDTO?> GetGroupByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateGroupAsync(CreateGroupDTO groupDTO);
        Task<(bool IsSuccess, string Message)> UpdateGroupAsync(int id, UpdateGroupDTO groupDTO);
        Task<(bool IsSuccess, string Message)> DeleteGroupAsync(int id);
    }
}
