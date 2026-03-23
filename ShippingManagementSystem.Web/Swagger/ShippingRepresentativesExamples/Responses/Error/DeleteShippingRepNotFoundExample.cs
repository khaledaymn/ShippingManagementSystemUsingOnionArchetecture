using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Error
{
    public class DeleteShippingRepNotFoundExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 400,
            Message = "Shipping representative not found."
        };
    }
}
