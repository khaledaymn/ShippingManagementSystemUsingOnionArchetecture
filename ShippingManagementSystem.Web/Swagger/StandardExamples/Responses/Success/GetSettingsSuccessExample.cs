using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.StandardExamples.Responses.Success
{
    public class GetSettingsSuccessExample : IExamplesProvider<List<StandardDTO>>
    {
        public List<StandardDTO> GetExamples()
        {
            return new List<StandardDTO>
            {
                new StandardDTO
                {
                    Id = 1,
                    StandardWeight = 5,
                    KGprice = 10,
                    VillagePrice = 20,
                    IsDeleted = false
                }
            };
        }
    }
}
