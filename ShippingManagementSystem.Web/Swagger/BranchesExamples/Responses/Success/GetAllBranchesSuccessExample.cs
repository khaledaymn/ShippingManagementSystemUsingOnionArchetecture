using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success
{
    public class GetAllBranchesSuccessExample : IExamplesProvider<PaginationResponse<GetBranchDTO>>
    {
        public PaginationResponse<GetBranchDTO> GetExamples()
        {
            var data = new List<GetBranchDTO>
            {
                new GetBranchDTO
                {
                    Id = 1,
                    Name = "Main Branch - Cairo",
                    CreationDate = "2023-01-15",
                    IsDeleted = false,
                    Location = "Downtown Square, Building 5",
                    CityName = "Cairo"
                },
                new GetBranchDTO
                {
                    Id = 2,
                    Name = "Giza Hub",
                    CreationDate = "2023-05-20",
                    IsDeleted = false,
                    Location = "Al Ahram St, Giza",
                    CityName = "Giza"
                }
            };

            return new PaginationResponse<GetBranchDTO>(pageSize: 10, pageIndex: 1, totalCount: 2, data: data);
        }
    }
}
