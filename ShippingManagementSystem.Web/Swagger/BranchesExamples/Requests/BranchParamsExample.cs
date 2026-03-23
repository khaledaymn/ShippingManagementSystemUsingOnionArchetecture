using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Requests
{
    public class BranchParamsExample : IExamplesProvider<BranchParams>
    {
        public BranchParams GetExamples() => new BranchParams
        {
            Search = "Main",
            CityId = 1, // Cairo
            Location = "Downtown",
            IsDeleted = false,
            PageIndex = 1,
            PageSize = 10,
            Sort = "name_asc"
        };
    }
}
