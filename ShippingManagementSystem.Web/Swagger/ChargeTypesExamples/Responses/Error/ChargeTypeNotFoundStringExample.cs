using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeNotFoundStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type with ID 99 not found";
    }
}
