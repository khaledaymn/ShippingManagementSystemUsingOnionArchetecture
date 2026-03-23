using Swashbuckle.AspNetCore.Filters;
namespace ShippingManagementSystem.Web.Swagger.DashboardExamples.Responses.Success
{

    namespace ShippingManagementSystem.Web.Swagger.Examples
    {
        public class DashboardInternalErrorExample : IExamplesProvider<string>
        {
            public string GetExamples() => "An unexpected error occurred while retrieving the dashboard summary: Database connection timed out.";
        }
    }
}
