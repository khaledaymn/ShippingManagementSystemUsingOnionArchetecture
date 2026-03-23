using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ChargeTypesExamples.Responses.Success
{
    public class ChargeTypeDeleteSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Charge type 'Express Shipping' deleted successfully";
    }
}
