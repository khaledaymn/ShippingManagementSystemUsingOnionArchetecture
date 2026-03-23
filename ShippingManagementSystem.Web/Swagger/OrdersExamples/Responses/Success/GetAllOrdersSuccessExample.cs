using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class GetAllOrdersSuccessExample : IExamplesProvider<PaginationResponse<OrderDTO>>
    {
        public PaginationResponse<OrderDTO> GetExamples()
        {
            var data = new List<OrderDTO>
            {
                new OrderDTO
                {
                    Id = 101,
                    CustomerName = "Ahmed Ali",
                    CustomerPhone1 = "01012345678",
                    OrderState = "New",
                    OrderPrice = 1500,
                    ChargePrice = 50,
                    AmountReceived = 1550,
                    CityName = "Cairo",
                    CreationDate = "2025-08-11 10:00:00"
                },
                new OrderDTO
                {
                    Id = 102,
                    CustomerName = "Sara Mohamed",
                    CustomerPhone1 = "01198765432",
                    OrderState = "Delivered",
                    OrderPrice = 800,
                    ChargePrice = 40,
                    AmountReceived = 840,
                    CityName = "Giza",
                    CreationDate = "2025-08-11 11:30:00"
                }
            };

            return new PaginationResponse<OrderDTO>(pageSize: 10, pageIndex: 1, totalCount: 2, data: data);
        }
    }
}
