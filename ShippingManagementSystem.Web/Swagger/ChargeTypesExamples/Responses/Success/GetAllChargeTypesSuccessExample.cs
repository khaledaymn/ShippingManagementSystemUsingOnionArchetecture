using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Success
{
    public class GetAllChargeTypesSuccessExample : IExamplesProvider<PaginationResponse<ChargeTypeDTO>>
    {
        public PaginationResponse<ChargeTypeDTO> GetExamples()
        {
            var data = new List<ChargeTypeDTO>
            {
                new ChargeTypeDTO
                {
                    Id = 1,
                    Name = "Standard Shipping",
                    ExtraPrice = 0.0,
                    NumOfDay = 5,
                    IsDeleted = false
                },
                new ChargeTypeDTO
                {
                    Id = 2,
                    Name = "Express Shipping",
                    ExtraPrice = 30.0,
                    NumOfDay = 2,
                    IsDeleted = false
                }
            };

            return new PaginationResponse<ChargeTypeDTO>(pageSize: 100, pageIndex: 1, totalCount: 2, data: data);
        }
    }
}
