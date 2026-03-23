using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.CityDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Success
{
    public class GetAllCitiesSuccessExample : IExamplesProvider<PaginationResponse<CityDTO>>
    {
        public PaginationResponse<CityDTO> GetExamples()
        {
            var data = new List<CityDTO>
            {
                new CityDTO
                {
                    Id = 10,
                    Name = "6th of October",
                    ChargePrice = 45.0,
                    PickUpPrice = 15.0,
                    GovernorateName = "Giza",
                    IsDeleted = false
                },
                new CityDTO
                {
                    Id = 11,
                    Name = "Sheikh Zayed",
                    ChargePrice = 50.0,
                    PickUpPrice = 15.0,
                    GovernorateName = "Giza",
                    IsDeleted = false
                }
            };

            return new PaginationResponse<CityDTO>(pageSize: 20, pageIndex: 1, totalCount: 2, data: data);
        }
    }
}
