using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeDeleteBadRequestExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Invalid charge type ID";
    }
}
