using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Requests
{
    public class UpdateBranchRequestExample : IExamplesProvider<BranchDTO>
    {
        public BranchDTO GetExamples() => new BranchDTO
        {
            Id = 5,
            Name = null, 
            Location = "New Address: 45 Industrial Zone",
            CityId = null
        };
    }
}
