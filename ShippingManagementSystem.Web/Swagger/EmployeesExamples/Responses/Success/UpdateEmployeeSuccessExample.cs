using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.EmployeesExamples.Responses.Success
{
    public class UpdateEmployeeSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Employee updated successfully.";
    }
}
