using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonDeleteNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Rejected reason with id 12 not found";
    }
}
