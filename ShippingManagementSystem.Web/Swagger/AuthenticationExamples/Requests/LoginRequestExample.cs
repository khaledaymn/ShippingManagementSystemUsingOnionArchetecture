using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests
{
    public class LoginRequestExample : IExamplesProvider<LoginDTO>
    {
        public LoginDTO GetExamples()
        {
            return new LoginDTO
            {
                Email = "admin@gmail.com",
                Password = "P@ssword123"
            };
        }
    }
}
