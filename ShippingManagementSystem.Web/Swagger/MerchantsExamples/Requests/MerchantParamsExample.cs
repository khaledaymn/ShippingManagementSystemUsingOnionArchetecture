using ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Requests
{
    public class MerchantParamsExample : IExamplesProvider<MerchantParams>
    {
        public MerchantParams GetExamples() => new MerchantParams
        {
            Search = "Ayman Tech",
            IsActive = true,
            Sort = "name_asc",
            PageIndex = 1,
            PageSize = 10
        };
    }
}
