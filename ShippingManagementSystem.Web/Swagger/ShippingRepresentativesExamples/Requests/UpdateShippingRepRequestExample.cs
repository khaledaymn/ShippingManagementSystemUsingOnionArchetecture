using ShippingManagementSystem.Application.UserTypes.Enums;
using ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.ShippingRepresentativesExamples.Requests
{
    public class UpdateShippingRepRequestExample : IExamplesProvider<UpdateShippingRepresentativeDTO>
    {
        public UpdateShippingRepresentativeDTO GetExamples() => new UpdateShippingRepresentativeDTO
        {
            Id = "rep_uuid_101",
            Name = "Mahmoud A. El-Sayed",
            PhoneNumber = "01055667788",
            DiscountType = DiscountType.Fixed,
            CompanyPercentage = 50.0,
            GovernorateIds = new List<int> { 1, 2, 6 } // Adding new territories
        };
    }
}
