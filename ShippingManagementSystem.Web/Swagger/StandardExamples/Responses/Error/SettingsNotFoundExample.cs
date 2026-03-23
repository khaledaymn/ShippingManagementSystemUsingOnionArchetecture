using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.StandardExamples.Responses.Error
{
    public class SettingsNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "No standards found";
    }
}
