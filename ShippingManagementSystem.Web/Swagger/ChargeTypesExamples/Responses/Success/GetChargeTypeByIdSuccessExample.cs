using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Success
{
    public class GetChargeTypeByIdSuccessExample : IExamplesProvider<ChargeTypeDTO>
    {
        public ChargeTypeDTO GetExamples() => new ChargeTypeDTO
        {
            Id = 1,
            Name = "Express Shipping",
            ExtraPrice = 30.0,
            NumOfDay = 2,
            IsDeleted = false
        };
    }
}
