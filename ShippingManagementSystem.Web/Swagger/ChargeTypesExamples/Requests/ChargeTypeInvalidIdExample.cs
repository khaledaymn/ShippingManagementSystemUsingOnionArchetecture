using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{
    public class ChargeTypeInvalidIdExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Invalid charge type ID";
    }
}
