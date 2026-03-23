using ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{
    public class ChargeTypeParamsExample : IExamplesProvider<ChargeTypeParams>
    {
        public ChargeTypeParams GetExamples() => new ChargeTypeParams
        {
            Search = "Express",
            MinPrice = 10,
            MaxPrice = 50,
            MinDays = 1,
            MaxDays = 3,
            IsDeleted = false,
            PageIndex = 1,
            PageSize = 10,
            Sort = "price_asc"
        };
    }
}
