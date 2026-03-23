using ShippingManagementSystem.Application.UserTypes.Enums;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Success
{
    public class GetShippingRepByIdSuccessExample : IExamplesProvider<ShippingRepresentativeDTO>
    {
        public ShippingRepresentativeDTO GetExamples() => new ShippingRepresentativeDTO
        {
            Id = "rep_uuid_101",
            Name = "Mahmoud El-Sayed",
            Email = "mahmoud.rep@shipping.com",
            PhoneNumber = "01022334455",
            Address = "45 Al-Horreya St, Menofia",
            DiscountType = DiscountType.Persentage,
            CompanyPercentage = 12.5,
            HiringDate = new DateTime(2025, 01, 15),
            Governorates = new List<string> { "Cairo", "Giza", "Menofia" },
            IsDeleted = false
        };
    }
}
