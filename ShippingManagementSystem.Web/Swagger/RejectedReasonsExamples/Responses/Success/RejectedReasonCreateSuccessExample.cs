using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Success
{
    public class RejectedReasonCreateSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Rejected reason created successfully";
    }
}
