using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{
    public class UpdateChargeTypeRequestExample : IExamplesProvider<ChargeTypeDTO>
    {
        public ChargeTypeDTO GetExamples() => new ChargeTypeDTO
        {
            Id = 1,
            ExtraPrice = 45.0, 
            Name = null,       
            NumOfDay = 0       
        };
    }
}
