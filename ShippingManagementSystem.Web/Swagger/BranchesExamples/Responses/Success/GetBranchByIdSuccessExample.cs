using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success
{
    public class GetBranchByIdSuccessExample : IExamplesProvider<GetBranchDTO>
    {
        public GetBranchDTO GetExamples() => new GetBranchDTO
        {
            Id = 1,
            Name = "Main Branch - Cairo",
            CreationDate = "2023-01-15",
            IsDeleted = false,
            Location = "Downtown Square, Building 5",
            CityName = "Cairo"
        };
    }
}
