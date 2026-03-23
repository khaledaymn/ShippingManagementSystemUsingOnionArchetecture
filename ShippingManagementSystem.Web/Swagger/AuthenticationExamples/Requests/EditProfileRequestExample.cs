using ShippingManagementSystem.Domain.DTOs.AuthenticationDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.AuthenticationExamples.Requests
{
    public class EditProfileRequestExample : IExamplesProvider<SpecificUserDataDTO>
    {
        public SpecificUserDataDTO GetExamples() => new SpecificUserDataDTO
        {
            Id = "usr_12345",
            Name = "Khaled Ayman Updated",
            Address = "New Address, District 5, Cairo",
            PhoneNumber = "01099887766",
            Email = "new.email@example.com"
        };
    }
}
