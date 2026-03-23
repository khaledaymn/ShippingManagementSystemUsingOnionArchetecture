using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Success
{
    public class GroupDeleteSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group 'Temporary Staff' deleted successfully";
    }
}
