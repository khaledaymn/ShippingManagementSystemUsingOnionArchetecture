using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Success
{
    public class GetAllGroupsSuccessExample : IExamplesProvider<PaginationResponse<GroupDTO>>
    {
        public PaginationResponse<GroupDTO> GetExamples()
        {
            var data = new List<GroupDTO>
            {
                new GroupDTO { Id = 1, Name = "Full Access Admin", CreationDate = "2024-01-10 10:00" },
                new GroupDTO { Id = 2, Name = "Branch Manager", CreationDate = "2024-02-15 12:30" },
                new GroupDTO { Id = 3, Name = "Data Entry Clerk", CreationDate = "2024-03-20 09:15" }
            };
            return new PaginationResponse<GroupDTO>(pageSize: 20, pageIndex: 1, totalCount: 3, data: data);
        }
    }
}
