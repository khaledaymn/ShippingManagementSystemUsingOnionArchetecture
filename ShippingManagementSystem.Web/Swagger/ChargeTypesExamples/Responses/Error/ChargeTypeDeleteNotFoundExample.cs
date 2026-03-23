using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeDeleteNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type with id 99 not found";
    }
}
