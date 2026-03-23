using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonCreateInternalErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error creating rejected reason: Database connection failed";
    }
}
