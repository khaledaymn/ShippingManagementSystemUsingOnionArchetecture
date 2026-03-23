using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Error
{
    public class RejectedReasonBadRequestExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Invalid rejected reason parameters";
    }
}
