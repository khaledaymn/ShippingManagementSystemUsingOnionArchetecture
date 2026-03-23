using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Swagger.GroupsExamples.Responses.Success
{
    public class GroupCreateSuccessExample : IExamplesProvider<string>
    {
        public string GetExamples() => "Group 'Accounting & Finance Team' created successfully";
    }
}
