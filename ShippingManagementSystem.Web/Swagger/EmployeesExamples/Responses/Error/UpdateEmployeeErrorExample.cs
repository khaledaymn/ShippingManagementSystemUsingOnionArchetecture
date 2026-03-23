using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Error
{
    public class UpdateEmployeeErrorExample : IExamplesProvider<object>
    {
        public object GetExamples() => new { Message = "Employee not found." };
    }
}
