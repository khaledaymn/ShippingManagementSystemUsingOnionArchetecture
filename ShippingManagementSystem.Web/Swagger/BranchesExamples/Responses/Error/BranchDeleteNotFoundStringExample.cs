using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchDeleteNotFoundStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch with id 99 not found";
    }
}
