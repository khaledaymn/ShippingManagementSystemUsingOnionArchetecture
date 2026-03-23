using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonCreateBadRequestExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error creating rejected reason: A reason with the same text already exists.";
    }
}
