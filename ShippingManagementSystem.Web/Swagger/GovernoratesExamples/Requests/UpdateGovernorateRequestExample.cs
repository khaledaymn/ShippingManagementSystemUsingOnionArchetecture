using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Requests
{
    public class UpdateGovernorateRequestExample : IExamplesProvider<GovernorateDTO>
    {
        public GovernorateDTO GetExamples() => new GovernorateDTO
        {
            Id = 5,
            Name = "New Alexandria Name",
            IsDeleted = false
        };
    }
}
