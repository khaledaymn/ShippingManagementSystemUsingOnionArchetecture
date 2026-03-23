using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Requests
{
    public class UpdateOrderStatusRequestExample : IExamplesProvider<UpdateOrderStatusDTO>
    {
        public UpdateOrderStatusDTO GetExamples() => new UpdateOrderStatusDTO
        {
            OrderState = "Delivered",
            Notes = "The customer received the package and paid the full amount."
        };
    }
}
