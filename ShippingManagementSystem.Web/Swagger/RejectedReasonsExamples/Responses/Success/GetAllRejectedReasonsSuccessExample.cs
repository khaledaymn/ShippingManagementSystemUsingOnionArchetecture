using ShippingManagementSystem.Domain.DTOs;
using ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Success
{
    public class GetAllRejectedReasonsSuccessExample : IExamplesProvider<PaginationResponse<RejectedReasonDTO>>
    {
        public PaginationResponse<RejectedReasonDTO> GetExamples()
        {
            var data = new List<RejectedReasonDTO>
            {
                new RejectedReasonDTO { Id = 1, Text = "Customer Refused Delivery", IsDeleted = false },
                new RejectedReasonDTO { Id = 2, Text = "Incorrect Address Provided", IsDeleted = false }
            };
            return new PaginationResponse<RejectedReasonDTO>(100, 1, 2, data);
        }
    }
}
