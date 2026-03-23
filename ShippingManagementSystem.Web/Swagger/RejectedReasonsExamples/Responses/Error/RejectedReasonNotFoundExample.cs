using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Rejected reason with ID 99 not found";
    }
}
