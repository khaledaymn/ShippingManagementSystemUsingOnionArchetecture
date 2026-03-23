using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.RejectedReasonsExamples.Responses.Success
{
    public class RejectedReasonDeleteSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Rejected reason deleted successfully";
    }
}
