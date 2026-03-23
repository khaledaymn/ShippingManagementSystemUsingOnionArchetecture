using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.GovernorateDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Success
{
    public class GetAllGovernoratesSuccessExample : IExamplesProvider<PaginationResponse<GovernorateDTO>>
    {
        public PaginationResponse<GovernorateDTO> GetExamples()
        {
            var data = new List<GovernorateDTO>
            {
                new GovernorateDTO { Id = 1, Name = "Cairo", IsDeleted = false },
                new GovernorateDTO { Id = 2, Name = "Giza", IsDeleted = false },
                new GovernorateDTO { Id = 3, Name = "Alexandria", IsDeleted = false }
            };

            return new PaginationResponse<GovernorateDTO>(pageSize: 10, pageIndex: 1, totalCount: 3, data: data);
        }
    }
}
