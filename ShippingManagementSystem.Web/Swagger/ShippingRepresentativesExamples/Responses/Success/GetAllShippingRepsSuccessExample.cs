using ShippingManagementSystem.Application.UserTypes.Enums;
using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Responses.Success
{
    public class GetAllShippingRepsSuccessExample : IExamplesProvider<PaginationResponse<ShippingRepresentativeDTO>>
    {
        public PaginationResponse<ShippingRepresentativeDTO> GetExamples()
        {
            var data = new List<ShippingRepresentativeDTO>
            {
                new ShippingRepresentativeDTO
                {
                    Id = "rep_uuid_101",
                    Name = "Mahmoud El-Sayed",
                    Email = "mahmoud.rep@shipping.com",
                    PhoneNumber = "01022334455",
                    Address = "45 Al-Horreya St, Menofia",
                    DiscountType = DiscountType.Persentage,
                    CompanyPercentage = 12.5,
                    HiringDate = DateTime.Now.AddMonths(-3),
                    Governorates = new List<string> { "Cairo", "Giza", "Menofia" },
                    IsDeleted = false
                }
            };

            return new PaginationResponse<ShippingRepresentativeDTO>(pageSize: 10, pageIndex: 1, totalCount: 1, data: data);
        }
    }
}
