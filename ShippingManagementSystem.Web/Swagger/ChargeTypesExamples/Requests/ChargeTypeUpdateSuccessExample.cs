using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Requests
{
    public class ChargeTypeUpdateSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type 'Express' updated successfully";
    }
}
