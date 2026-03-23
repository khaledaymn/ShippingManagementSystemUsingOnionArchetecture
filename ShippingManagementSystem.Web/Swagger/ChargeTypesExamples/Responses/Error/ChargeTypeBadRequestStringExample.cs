using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeBadRequestStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Invalid charge type parameters";
    }
}
