using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Requests
{
    public class CreateRejectedReasonRequestExample : IExamplesProvider<CreateRejectedReasonDTO>
    {
        public CreateRejectedReasonDTO GetExamples() => new CreateRejectedReasonDTO
        {
            Text = "Customer's phone is switched off"
        };
    }
}
