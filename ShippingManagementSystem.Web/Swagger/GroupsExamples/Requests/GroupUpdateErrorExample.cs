using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests
{
    public class GroupUpdateErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group with id 3 not found";
    }
}
