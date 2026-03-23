using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Error
{
    public class AddEmployeeLogicErrorExample : IExamplesProvider<object>
    {
        public object GetExamples() => new { Message = "Email already exists." };
    }
}
