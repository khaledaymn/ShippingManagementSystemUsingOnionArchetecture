using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.MerchantsExamples.Responses.Success
{
    public class UpdateMerchantSuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "Merchant updated successfully."
        };
    }
}
