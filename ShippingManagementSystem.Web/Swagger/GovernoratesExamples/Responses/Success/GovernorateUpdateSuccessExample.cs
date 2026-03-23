using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Success
{
    public class GovernorateUpdateSuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "Governorate 'Cairo' updated successfully"
        };
    }
}
