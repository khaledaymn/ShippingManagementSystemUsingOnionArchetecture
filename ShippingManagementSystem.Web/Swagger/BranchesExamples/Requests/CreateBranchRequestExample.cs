using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Requests
{
    public class CreateBranchRequestExample : IExamplesProvider<CreateBranchDTO>
    {
        public CreateBranchDTO GetExamples() => new CreateBranchDTO
        {
            Name = "Main Giza Hub",
            Location = "Al Ahram St, Near Giza Square",
            CityId = 2 // Giza
        };
    }
}
