using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupCreateErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Permissions are required to create a group";
    }
}
