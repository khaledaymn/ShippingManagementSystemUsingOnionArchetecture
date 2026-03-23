using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.DTOs.DashboardDTOs;
using ShippingManagementSystem.Web.Swagger.DashboardExamples.Responses.Success;
using ShippingManagementSystem.Web.Swagger.DashboardExamples.Responses.Success.ShippingManagementSystem.Web.Swagger.Examples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ShippingManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("~/Dashboard")]
        [SwaggerOperation(
            OperationId = "GetDashboardAnalytics",
            Summary = "Generate real-time administrative and financial analytics",
            Description = "### 📊 Business Intelligence Engine\n\n" +
                            "This endpoint functions as the central nervous system for the dashboard. It calculates **Time-Series Data** and **Performance KPIs** by comparing the current month's performance against the previous period.\n\n" +
                            "#### **⚙️ Analytical Components:**\n" +
                            "* **Trend Analysis**: Fields prefixed with `Change` (e.g., `ChangeEfficiency`) represent the delta between the current and previous month, used for rendering 'Up/Down' trend indicators.\n" +
                            "* **State Distribution**: A categorical breakdown of all orders, perfectly structured for **Donut/Pie charts**.\n" +
                            "* **Financial Metrics**: Aggregates revenue and average shipping costs to monitor profitability.\n" +
                            "* **Annual Performance**: A 12-month percentage distribution for line chart visualization.\n\n" +
                            "#### **👤 Contextual Filtering:**\n" +
                            "* **Global View**: If `userId` is null, it returns system-wide stats (Admin view).\n" +
                            "* **Scoped View**: If `userId` is provided, the analytics are filtered specifically for that **Merchant** or **Shipping Representative**.\n\n" +
                            "#### **⚠️ Reliability Matrix:**\n" +
                            "| Status | Scenario | Format |\n" +
                            "| :--- | :--- | :--- |\n" +
                            "| **200 OK** | Full analytics payload calculated. | `DashboardSummaryDTO (JSON)` |\n" +
                            "| **500 Err** | Failure in complex aggregation or DB timeout. | `Plain Text String` |\n\n" +
                            "> **Note:** Calculation is based on `UTC` time. Data includes all non-deleted orders linked to the user context.",
            Tags = new[] { "5. Administrative Control & Analytics" }
        )]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DashboardSummarySuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(DashboardInternalErrorExample))]
        [ProducesResponseType(typeof(DashboardSummaryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummary([FromQuery]string? userId)
        {
            try
            {
                var summary = await _unitOfWork.DashboardService.GetDashboardSummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                // Log exception here (e.g., using ILogger)
                return StatusCode(500, "An unexpected error occurred while retrieving the dashboard summary: " + ex.Message);
                //"An unexpected error occurred while retrieving the dashboard summary"
            }
        }
    }
}
