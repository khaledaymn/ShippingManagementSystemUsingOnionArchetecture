using ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Requests
{
    public class RejectedReasonParamsExample : IExamplesProvider<RejectedReasonParams>
    {
        public RejectedReasonParams GetExamples() => new RejectedReasonParams
        {
            Search = "Customer",
            IsDeleted = false,
            Sort = "text_asc",
            PageIndex = 1,
            PageSize = 20
        };
    }
}
