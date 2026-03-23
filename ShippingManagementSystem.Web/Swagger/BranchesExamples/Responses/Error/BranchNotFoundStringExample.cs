using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchNotFoundStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch with ID 99 not found";
    }
}
