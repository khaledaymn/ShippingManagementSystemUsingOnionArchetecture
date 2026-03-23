using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Error
{
    public class CityNegativePriceErrorExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Pick-up price cannot be negative"
        };
    }
}
