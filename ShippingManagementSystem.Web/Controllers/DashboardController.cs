using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingManagementSystem.Application.UnitOfWork;

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
