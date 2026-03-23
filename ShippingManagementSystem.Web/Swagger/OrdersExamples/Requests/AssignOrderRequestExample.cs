using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Requests
{
    public class AssignOrderRequestExample : IExamplesProvider<AssignOrderToDelivaryDTO>
    {
        public AssignOrderToDelivaryDTO GetExamples() => new AssignOrderToDelivaryDTO
        {
            OrderState = "DeliveredToTheRepresentative",
            ShippingRepresentativeId = "usr_rep_10"
        };
    }
}
