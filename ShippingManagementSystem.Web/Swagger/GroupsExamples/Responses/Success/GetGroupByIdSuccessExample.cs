using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Success
{
    public class GetGroupByIdSuccessExample : IExamplesProvider<GroupPermissionsDTO>
    {
        public GroupPermissionsDTO GetExamples() => new GroupPermissionsDTO
        {
            Id = 3,
            Name = "Regional Supervisors",
            Date = "09/07/2025",
            Permissions = new Dictionary<string, List<int>>
            {
                { "Orders", new List<int> { 1, 2, 3, 4 } }, 
                { "Branches", new List<int> { 1, 2 } },     
                { "Reports", new List<int> { 1 } }          
            }
        };
    }
}
