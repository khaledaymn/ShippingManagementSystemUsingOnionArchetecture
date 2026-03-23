using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests
{
    public class UpdateGroupRequestExample : IExamplesProvider<UpdateGroupDTO>
    {
        public UpdateGroupDTO GetExamples() => new UpdateGroupDTO
        {
            Id = 3,
            Name = "Senior Accountants (Revised)",
            Permissions = new List<Permission>
            {
                new Permission { Id = 1, Values = new List<int> { 1, 2, 3, 4 } }, 
                new Permission { Id = 2, Values = new List<int> { 1 } }           
            }
        };
    }
}
