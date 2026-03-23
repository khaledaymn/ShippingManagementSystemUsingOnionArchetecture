using ShippingManagementSystem.Application.UserTypes.Enums;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Requests
{
    public class AddShippingRepRequestExample : IExamplesProvider<AddShippingRepresentativeDTO>
    {
        public AddShippingRepresentativeDTO GetExamples() => new AddShippingRepresentativeDTO
        {
            Name = "Mahmoud Delivery",
            Email = "mahmoud.rep@express.com",
            PhoneNumber = "01022334455",
            Password = "Rep@Password2026",
            Address = "45 Al-Horreya St, Menofia",
            DiscountType = DiscountType.Persentage,
            CompanyPercentage = 15.0,
            GovernorateIds = new List<int> { 1, 3, 5 } // Cairo, Alexandria, Menofia
        };
    }
}
