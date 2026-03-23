using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.BranchesExamples.Responses.Success
{
    public class BranchCreateSuccessStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Branch 'Main Giza Hub' created successfully";
    }
}
