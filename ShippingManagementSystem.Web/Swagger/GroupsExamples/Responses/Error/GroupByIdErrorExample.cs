using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupByIdErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error retrieving group: An internal error occurred during mapping.";
    }
}
