using ShippingManagementSystem.Domain.DTOs.GroupDTOs;
using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests
{
    public class CreateGroupRequestExample : IExamplesProvider<CreateGroupDTO>
    {
        public CreateGroupDTO GetExamples() => new CreateGroupDTO
        {
            Name = "Accounting & Finance Team",
            Permissions = new List<Permission>
            {
                new Permission { Id = 1, Values = new List<int> { 1, 3 } }, 
                new Permission { Id = 5, Values = new List<int> { 1, 2, 3, 4 } } 
            }
        };
    }
}
