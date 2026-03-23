using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success
{
    public class AddEmployeeSuccessExample : IExamplesProvider<object>
    {
        public object GetExamples() => new { Message = "Employee created successfully." };
    }
}
