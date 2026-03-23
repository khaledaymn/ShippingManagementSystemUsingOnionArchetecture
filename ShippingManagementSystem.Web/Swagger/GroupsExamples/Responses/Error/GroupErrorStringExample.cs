using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupErrorStringExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error retrieving groups: Database connection failed.";
    }
}
