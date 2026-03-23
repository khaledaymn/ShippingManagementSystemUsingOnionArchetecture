using ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Requests
{
    public class EmployeeParamsExample : IExamplesProvider<EmployeeParams>
    {
        public EmployeeParams GetExamples() => new EmployeeParams
        {
            Search = "Ahmed",
            IsActive = true,
            Sort = "name_asc",
            PageIndex = 1,
            PageSize = 10
        };
    }
}
