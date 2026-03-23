using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class GetOrdersByStatusSuccessExample : IExamplesProvider<List<OrderDTO>>
    {
        public List<OrderDTO> GetExamples()
        {
            return new List<OrderDTO>
            {
                new OrderDTO
                {
                    Id = 501,
                    CustomerName = "Yassine Mahmoud",
                    OrderState = "Delivered",
                    OrderPrice = 1200.0,
                    ChargePrice = 50.0,
                    AmountReceived = 1250.0,
                    CityName = "Alexandria",
                    CreationDate = "2025-08-10 09:00:00"
                },
                new OrderDTO
                {
                    Id = 505,
                    CustomerName = "Mariam Hassan",
                    OrderState = "Delivered",
                    OrderPrice = 450.0,
                    ChargePrice = 30.0,
                    AmountReceived = 480.0,
                    CityName = "Cairo",
                    CreationDate = "2025-08-11 15:20:00"
                }
            };
        }
    }
}
