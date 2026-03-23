using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDtos;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Success
{
    public class GetMerchantByIdSuccessExample : IExamplesProvider<MerchantDTO>
    {
        public MerchantDTO GetExamples() => new MerchantDTO
        {
            Id = "usr_merch_77",
            Name = "Ayman Ahmed",
            StoreName = "Ayman Logistics",
            Email = "ayman@store.com",
            PhoneNumber = "01098684485",
            Address = "15 Tahrir St, Cairo",
            IsDeleted = false,
            StartWorkDate = new DateTime(2024, 05, 10),
            SpecialPickUp = 25,
            RejectedOrderPrecentage = 1.2,
            Branches = new List<GetBranchDTO>
            {
                new GetBranchDTO { Id = 1, Name = "Cairo Branch", CityName = "Cairo", Location = "Downtown" },
                new GetBranchDTO { Id = 2, Name = "Giza Branch", CityName = "Giza", Location = "Pyramids Area" }
            },
            SpecialDeliveryPrices = new List<SpecialDeliveryPriceDTO>
            {
                new SpecialDeliveryPriceDTO { cityId = 3, CityName = "Alexandria", SpecialPrice = 40.0 },
                new SpecialDeliveryPriceDTO { cityId = 4, SpecialPrice = 50.0 }
            }
        };
    }
}
