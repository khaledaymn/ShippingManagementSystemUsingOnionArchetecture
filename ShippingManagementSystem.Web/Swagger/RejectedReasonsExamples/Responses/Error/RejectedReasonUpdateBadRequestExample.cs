using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonUpdateBadRequestExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Rejected reason with id 5 not found";
    }
}
