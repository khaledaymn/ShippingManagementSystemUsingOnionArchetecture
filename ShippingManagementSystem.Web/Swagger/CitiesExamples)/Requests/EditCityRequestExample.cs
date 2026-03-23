using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Requests
{
    public class EditCityRequestExample : IExamplesProvider<EditCityDTO>
    {
        public EditCityDTO GetExamples() => new EditCityDTO
        {
            Id = 10,
            ChargePrice = 60.0, // Update price only
            Name = null,        // Keep old name
            GovernorateId = null // Keep old governorate
        };
    }
}
