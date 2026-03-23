using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Error
{
    public class InternalServerErrorExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples() => new ErrorResponse
        {
            StatusCode = 500,
            Message = "An error occurred while processing your request",
            Details = "SmtpCommandException: The operation has timed out."
        };
    }
}
