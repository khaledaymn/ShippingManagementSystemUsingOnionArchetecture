using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Error
{
    public class AssignOrderBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Order is in New state and cannot be assigned to delivery representative."
        };
    }
}
