using ShippingManagementSystem.Web.Swagger.Schemas;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.CitiesExamples_.Responses.Success
{
    public class CreateCitySuccessExample : IExamplesProvider<SuccessResponse>
    {
        public SuccessResponse GetExamples() => new SuccessResponse
        {
            Message = "City 'Nasr City' created successfully"
        };
    }
}
