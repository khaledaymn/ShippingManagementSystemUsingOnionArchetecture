using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{
    public class ChargeTypeUpdateNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type with id 5 not found";
    }
}
