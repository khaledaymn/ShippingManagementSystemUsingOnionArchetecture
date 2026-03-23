using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.DTOs.MerchantDtos;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Success
{
    public class GetAllMerchantsSuccessExample : IExamplesProvider<PaginationResponse<MerchantDTO>>
    {
        public PaginationResponse<MerchantDTO> GetExamples()
        {
            var data = new List<MerchantDTO>
            {
                new MerchantDTO
                {
                    Id = "usr_merch_01",
                    Name = "Khaled Ayman",
                    StoreName = "Ayman Tech Store",
                    Email = "khaled.ayman@example.com",
                    PhoneNumber = "01098684485",
                    Address = "123 Nile St, Cairo",
                    IsDeleted = false,
                    StartWorkDate = DateTime.Now.AddMonths(-6),
                    SpecialPickUp = 15,
                    RejectedOrderPrecentage = 1.5,
                    Branches = new List<GetBranchDTO>
                    {
                        new GetBranchDTO { Id = 1, Name = "Main Cairo", CityName = "Cairo", Location = "Downtown" }
                    },
                    SpecialDeliveryPrices = new List<SpecialDeliveryPriceDTO>
                    {
                        new SpecialDeliveryPriceDTO { cityId = 2, CityName = "Alexandria", SpecialPrice = 35.0 }
                    }
                }
            };

            return new PaginationResponse<MerchantDTO>(pageSize: 10, pageIndex: 1, totalCount: 1, data: data);
        }
    }
}
