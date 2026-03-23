using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Success
{
    public class GetCityByIdSuccessExample : IExamplesProvider<CityDTO>
    {
        public CityDTO GetExamples() => new CityDTO
        {
            Id = 10,
            Name = "6th of October",
            ChargePrice = 45.0,
            PickUpPrice = 15.0,
            GovernorateName = "Giza",
            IsDeleted = false
        };
    }
}
