using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Error
{
    public class CreateOrderBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "City with id 99 not found"
        };
    }
}
