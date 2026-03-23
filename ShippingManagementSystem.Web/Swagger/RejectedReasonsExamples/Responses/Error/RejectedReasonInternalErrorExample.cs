using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonInternalErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "An unexpected error occurred while retrieving rejected reasons";
    }
}
