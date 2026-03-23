using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Error
{
    public class AddMerchantBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Email already Exists"
        };
    }
}
