using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Error
{
    public class EmployeeErrorResponseExample : IExamplesProvider<object>
    {
        public object GetExamples() => new { Message = "An error occurred: [Specific Exception Message]" };
    }
}
