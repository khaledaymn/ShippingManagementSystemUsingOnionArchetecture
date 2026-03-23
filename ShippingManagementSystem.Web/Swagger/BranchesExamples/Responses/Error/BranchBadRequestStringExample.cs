using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Error
{
    public class BranchBadRequestStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Invalid branch parameters";
    }
}
