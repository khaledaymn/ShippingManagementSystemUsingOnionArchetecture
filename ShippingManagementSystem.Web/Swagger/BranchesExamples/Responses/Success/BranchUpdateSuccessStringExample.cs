using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success
{
    public class BranchUpdateSuccessStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch 'Main Hub' updated successfully";
    }
}
