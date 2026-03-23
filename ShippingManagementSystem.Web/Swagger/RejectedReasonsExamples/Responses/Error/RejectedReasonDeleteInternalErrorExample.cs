using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonDeleteInternalErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error deleting rejected reason: The record is associated with existing orders.";
    }
}
