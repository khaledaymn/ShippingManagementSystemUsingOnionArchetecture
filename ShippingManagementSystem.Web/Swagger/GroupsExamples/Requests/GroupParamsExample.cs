using ShippingManagementSystem.Domain.Specifications.GroupSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests
{
    public class GroupParamsExample : IExamplesProvider<GroupParams>
    {
        public GroupParams GetExamples() => new GroupParams
        {
            Search = "Accountant",
            Sort = "date_desc",
            PageIndex = 1,
            PageSize = 20,
            FromDate = DateTime.Now.AddMonths(-1),
            ToDate = DateTime.Now
        };
    }
}
