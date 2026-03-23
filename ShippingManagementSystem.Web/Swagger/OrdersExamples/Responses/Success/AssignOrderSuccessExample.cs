using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class AssignOrderSuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "Order status updated successfully"
        };
    }
}
