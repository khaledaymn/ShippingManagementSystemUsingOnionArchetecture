using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchCreateBadRequestExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch data is required";
    }
}
