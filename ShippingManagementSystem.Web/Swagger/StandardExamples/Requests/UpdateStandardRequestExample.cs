using ShippingManagementSystem.Domain.DTOs.StandardDTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.StandardExamples.Requests
{
    public class UpdateStandardRequestExample : IExamplesProvider<UpdateStandardDTO>
    {
        public UpdateStandardDTO GetExamples() => new UpdateStandardDTO
        {
            StandardWeight = 5, 
            KGprice = 15        
        };
    }
}
