using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Success
{
    public class GetGovernorateByIdSuccessExample : IExamplesProvider<GovernorateDTO>
    {
        public GovernorateDTO GetExamples() => new GovernorateDTO
        {
            Id = 1,
            Name = "Cairo",
            IsDeleted = false
        };
    }
}
