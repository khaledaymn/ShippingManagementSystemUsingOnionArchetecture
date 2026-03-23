using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Requests
{
    public class UpdateRejectedReasonRequestExample : IExamplesProvider<RejectedReasonDTO>
    {
        public RejectedReasonDTO GetExamples() => new RejectedReasonDTO
        {
            Id = 5,
            Text = "Updated: Recipient's phone is unreachable after 3 attempts",
            IsDeleted = false
        };
    }
}
