using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error
{
    public class InternalErrorExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 500,
            Message = "An unexpected error occurred.",
            Details = "DbUpdateException: Database connection timed out."
        };
    }
}
