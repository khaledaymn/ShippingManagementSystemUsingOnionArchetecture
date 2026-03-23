using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchCreateErrorStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error creating branch: City with ID 99 not found";
    }
}
