using ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{

    public class CreateChargeTypeRequestExample : IExamplesProvider<CreateChargeTypeDTO>
    {
        public CreateChargeTypeDTO GetExamples() => new CreateChargeTypeDTO
        {
            Name = "Overnight Delivery",
            ExtraPrice = 50.0,
            NumOfDay = 1
        };
    }
}
