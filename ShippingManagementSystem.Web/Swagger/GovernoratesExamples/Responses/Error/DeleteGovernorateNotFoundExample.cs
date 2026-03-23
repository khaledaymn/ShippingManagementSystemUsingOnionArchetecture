using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GovernoratesExamples.Responses.Error
{
    public class DeleteGovernorateNotFoundExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 404,
            Message = "Governorate with id 99 not found"
        };
    }
}
