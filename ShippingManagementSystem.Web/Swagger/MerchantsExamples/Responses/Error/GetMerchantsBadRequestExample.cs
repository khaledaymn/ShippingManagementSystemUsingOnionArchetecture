using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Error
{
    public class GetMerchantsBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Invalid parameters: PageSize must be between 1 and 10, and Sort field 'invalid_field' is not supported."
        };
    }
}
