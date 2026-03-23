using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Success
{
    public class ChargeTypeCreateSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type 'Overnight Delivery' created successfully";
    }
}
