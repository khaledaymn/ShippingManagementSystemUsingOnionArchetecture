using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupDeleteDependencyErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group 'Admins' cannot be deleted because it has associated users.";
    }
}
