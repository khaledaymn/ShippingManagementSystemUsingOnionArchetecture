using ShippingManagementSystem.Application.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Responses.Success
{
    public class LoginSuccessResponseExample : IExamplesProvider<AuthenticationResponseDTO>
    {
        public AuthenticationResponseDTO GetExamples()
        {
            return new AuthenticationResponseDTO
            {
                Id = "admin-uuid-12345",
                Name = "System Administrator",
                Email = "admin@gmail.com",
                Role = "Admin",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Message = "Login successfully",
                Permissions = new Dictionary<string, List<string>>
                {
                    { "Orders", new List<string> { "View", "Create", "Edit" } },
                    { "Branches", new List<string> { "View", "Delete" } }
                }
            };
        }
    }
}
