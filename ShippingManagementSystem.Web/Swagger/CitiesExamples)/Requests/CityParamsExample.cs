using ShippingManagementSystem.Domain.Specifications.CitySpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Requests
{
    public class CityParamsExample : IExamplesProvider<CityParams>
    {
        public CityParams GetExamples() => new CityParams
        {
            Search = "October",
            GovernorateId = 2, // Giza
            MinChargePrice = 30,
            MaxChargePrice = 60,
            PageIndex = 1,
            PageSize = 10,
            Sort = "price_asc"
        };
    }
}
