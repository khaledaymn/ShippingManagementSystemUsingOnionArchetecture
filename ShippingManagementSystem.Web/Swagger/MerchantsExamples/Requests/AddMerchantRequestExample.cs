using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDtos;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Requests
{
    public class AddMerchantRequestExample : IExamplesProvider<AddMerchantDTO>
    {
        public AddMerchantDTO GetExamples() => new AddMerchantDTO
        {
            Name = "Ayman Logistics",
            Email = "info@ayman-logistics.com",
            PhoneNumber = "01099887766",
            Password = "Merchant@Password123",
            Address = "12 Main St, Giza",
            StoreName = "Ayman Store",
            RejectedOrderPrecentage = 2.0f,
            SpecialPickUp = 20,
            BranchesIds = new List<int> { 1, 2 },
            SpecialDeliveryPrices = new List<SpecialDeliveryPriceDTO>
            {
                new SpecialDeliveryPriceDTO { cityId = 3, SpecialPrice = 45.0 },
                new SpecialDeliveryPriceDTO { cityId = 4, SpecialPrice = 55.0 }
            }
        };
    }
}
