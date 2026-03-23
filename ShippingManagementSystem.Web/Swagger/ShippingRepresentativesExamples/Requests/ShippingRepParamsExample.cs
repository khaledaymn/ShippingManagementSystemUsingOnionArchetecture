using ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Requests
{
    public class ShippingRepParamsExample : IExamplesProvider<ShippingRepresentativeParams>
    {
        public ShippingRepresentativeParams GetExamples() => new ShippingRepresentativeParams
        {
            Search = "Mahmoud",
            PageIndex = 1,
            PageSize = 10,
            IsActive = true,
            Sort = "hiring_desc"
        };
    }
}
