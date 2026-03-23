using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests
{
    public class ForgetPasswordRequestExample : IExamplesProvider<ForgetPasswordDTO>
    {
        public ForgetPasswordDTO GetExamples() => new ForgetPasswordDTO
        {
            Email = "khaled.ayman@gmail.com"
        };
    }
}
