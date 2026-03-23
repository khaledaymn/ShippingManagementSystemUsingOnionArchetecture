using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Requests
{
    public class UpdateMerchantRequestExample : IExamplesProvider<UpdateMerchantDTO>
    {
        public UpdateMerchantDTO GetExamples() => new UpdateMerchantDTO
        {
            Id = "usr_merch_77",
            Name = "Ayman Logistics Updated",
            StoreName = "Ayman Super Store",
            BranchesId = new List<int> { 1, 3 }, // Updating branches
            SpecialDeliveryPrices = new List<SpecialDeliveryPriceDTO>
            {
                new SpecialDeliveryPriceDTO { cityId = 5, SpecialPrice = 60.0 }
            }
        };
    }
}
