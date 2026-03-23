using ShippingManagementSystem.Domain.Specifications.GovernorateSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Requests
{
    public class GovernorateParamsExample : IExamplesProvider<GovernorateParams>
    {
        public GovernorateParams GetExamples() => new GovernorateParams
        {
            Search = "Cairo",
            IsDeleted = false,
            Sort = "name_asc",
            PageIndex = 1,
            PageSize = 10
        };
    }
}
