using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests
{
    public class ChangePasswordRequestExample : IExamplesProvider<ChangePasswordDTO>
    {
        public ChangePasswordDTO GetExamples() => new ChangePasswordDTO
        {
            UserId = "usr_12345",
            OldPassword = "OldP@ssword123",
            NewPassword = "NewP@ssword2025!"
        };
    }
}
