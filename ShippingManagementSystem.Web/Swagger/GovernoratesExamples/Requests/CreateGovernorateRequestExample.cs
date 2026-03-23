using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Requests
{
    public class CreateGovernorateRequestExample : IExamplesProvider<CreateGovernorateDTO>
    {
        public CreateGovernorateDTO GetExamples() => new CreateGovernorateDTO
        {
            Name = "Alexandria"
        };
    }
}
