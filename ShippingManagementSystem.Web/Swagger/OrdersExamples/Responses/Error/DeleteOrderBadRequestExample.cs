using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Error
{
    public class DeleteOrderBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Order cannot be deleted as it is not in New state."
        };
    }
}
