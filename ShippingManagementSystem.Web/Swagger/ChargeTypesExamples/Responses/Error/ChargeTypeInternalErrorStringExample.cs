using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Error
{
    public class ChargeTypeInternalErrorStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "An error occurred while retrieving charge type with ID 1";
    }
}
