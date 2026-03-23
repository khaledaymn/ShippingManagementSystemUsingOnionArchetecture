using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupNotFoundStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group with ID 99 not found";
    }
}
