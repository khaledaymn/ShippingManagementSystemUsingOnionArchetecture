using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success
{
    public class DeleteEmployeeSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Employee deleted successfully.";
    }
}
