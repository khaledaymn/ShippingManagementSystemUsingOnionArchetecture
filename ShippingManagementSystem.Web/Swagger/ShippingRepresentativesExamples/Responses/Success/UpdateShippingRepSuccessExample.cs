using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Success
{
    public class UpdateShippingRepSuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "Shipping representative updated successfully."
        };
    }
}
