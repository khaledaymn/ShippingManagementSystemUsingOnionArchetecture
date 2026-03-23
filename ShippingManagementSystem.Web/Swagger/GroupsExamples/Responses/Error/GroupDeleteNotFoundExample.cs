using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupDeleteNotFoundExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group with id 15 not found";
    }
}
