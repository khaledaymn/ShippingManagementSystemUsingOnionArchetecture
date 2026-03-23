using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeCreateErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type with name 'Overnight Delivery' already exists";
    }
}
