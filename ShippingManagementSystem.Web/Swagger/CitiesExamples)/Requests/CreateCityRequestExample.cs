using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Requests
{
    public class CreateCityRequestExample : IExamplesProvider<CreateCityDTO>
    {
        public CreateCityDTO GetExamples() => new CreateCityDTO
        {
            Name = "Nasr City",
            ChargePrice = 35.0,
            PickUpPrice = 10.0,
            GovernorateId = 1 // Cairo
        };
    }
}
