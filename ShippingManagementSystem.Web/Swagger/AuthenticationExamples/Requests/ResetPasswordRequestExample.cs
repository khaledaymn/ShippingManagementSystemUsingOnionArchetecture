using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests
{
    public class ResetPasswordRequestExample : IExamplesProvider<ResetPasswordDTO>
    {
        public ResetPasswordDTO GetExamples() => new ResetPasswordDTO
        {
            Email = "khaled.ayman@gmail.com",
            Token = "AQAAANCMnd8BFdERjHoAwE...",
            Password = "SecureNewPassword123!"
        };
    }
}
