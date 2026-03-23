using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Responses.Success
{
    public class GetOrderByIdSuccessExample : IExamplesProvider<OrderDTO>
    {
        public OrderDTO GetExamples() => new OrderDTO
        {
            Id = 1025,
            CreationDate = "2025-08-11 14:30:00",
            CustomerName = "Omar Ahmed",
            CustomerPhone1 = "01099887766",
            CustomerPhone2 = "01122334455",
            VillageAndStreet = "Nile Street, Building 10",
            Notes = "Call before arrival",
            OrderState = "Pendding",
            OrderType = "DeliveryAtBranch",
            PaymentType = "CashOnDelivery",
            ChargePrice = 45.0,
            OrderPrice = 500.0,
            AmountReceived = 545.0,
            TotalWeight = 2500,
            IsDeleted = false,
            IsShippingToVillage = false,
            CityName = "Cairo",
            ChargeTypeName = "Express",
            BranchName = "Main Cairo Branch",
            MerchantName = "Tech Store Egypt",
            ShippingRepresentativeName = "Mahmoud Delivery",
            Products = new List<ProductDTO>
            {
                new ProductDTO { Id = 1, Name = "Wireless Mouse", Weight = 500, Quantity = 2, OrderId = 1025 },
                new ProductDTO { Id = 2, Name = "Mechanical Keyboard", Weight = 1500, Quantity = 1, OrderId = 1025 }
            }
        };
    }
}
