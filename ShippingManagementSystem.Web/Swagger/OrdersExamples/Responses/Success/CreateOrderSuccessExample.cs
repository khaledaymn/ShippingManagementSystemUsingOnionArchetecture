using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class CreateOrderSuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "Order created successfully with ID: 1025"
        };
    }
}
