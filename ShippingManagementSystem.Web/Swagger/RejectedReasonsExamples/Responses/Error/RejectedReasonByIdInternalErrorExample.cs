using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonByIdInternalErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "An unexpected error occurred while retrieving rejected reason with ID 5";
    }
}
