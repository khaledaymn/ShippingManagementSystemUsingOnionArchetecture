using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success
{
    public class BranchDeleteSuccessStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch 'Giza Hub' deleted successfully";
    }
}
