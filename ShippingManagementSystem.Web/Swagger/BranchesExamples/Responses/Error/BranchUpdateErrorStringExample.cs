using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchUpdateErrorStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch with id 5 not found";
    }
}
