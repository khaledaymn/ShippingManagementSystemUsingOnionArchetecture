using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Error
{
    public class GroupInternalServerErrorExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Error retrieving groups: An unexpected database error occurred while joining with GroupModules.";
    }
}
