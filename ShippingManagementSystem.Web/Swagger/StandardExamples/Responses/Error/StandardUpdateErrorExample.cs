using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.StandardExamples.Responses.Error
{
    public class StandardUpdateErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Standard with id 1 not found";
    }
}
