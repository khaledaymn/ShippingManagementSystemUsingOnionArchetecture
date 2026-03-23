using ShippingManagementSystem.Domain.DTOs.OrderDTOs;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.OrdersExamples.Requests
{
    public class CreateOrderRequestExample : IExamplesProvider<CreateOrderDTO>
    {
        public CreateOrderDTO GetExamples() => new CreateOrderDTO
        {
            CustomerName = "Zaid Ali",
            CustomerPhone1 = "01066554433",
            CustomerPhone2 = "01566778899",
            VillageAndStreet = "99 Gamal Abdelnasser St, Flat 4",
            Notes = "Please deliver after 4 PM",
            OrderPrice = 1200.0,
            ShippingToVillage = false,
            CityId = 3,
            ChargeTypeId = 1,
            BranchId = 2,
            MerchantId = "usr_merch_77",
            OrderType = "DeliveryAtBranch",
            PaymentType = "CashOnDelivery",
            Products = new List<ProductDTO>
            {
                new ProductDTO { Name = "Smart Watch", Weight = 200, Quantity = 1 },
                new ProductDTO { Name = "Leather Belt", Weight = 150, Quantity = 2 }
            }
        };
    }
}
