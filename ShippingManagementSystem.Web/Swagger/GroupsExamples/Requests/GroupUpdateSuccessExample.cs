using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Requests
{
    public class GroupUpdateSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group 'Senior Accountants (Revised)' updated successfully";
    }
}
