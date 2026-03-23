using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonUpdateInternalErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error updating rejected reason: An internal database error occurred";
    }
}
