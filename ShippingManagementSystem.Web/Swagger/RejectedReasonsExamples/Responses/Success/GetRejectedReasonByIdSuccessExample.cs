using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Success
{
    public class GetRejectedReasonByIdSuccessExample : IExamplesProvider<RejectedReasonDTO>
    {
        public RejectedReasonDTO GetExamples() => new RejectedReasonDTO
        {
            Id = 5,
            Text = "Customer was not present at the location",
            IsDeleted = false
        };
    }
}
