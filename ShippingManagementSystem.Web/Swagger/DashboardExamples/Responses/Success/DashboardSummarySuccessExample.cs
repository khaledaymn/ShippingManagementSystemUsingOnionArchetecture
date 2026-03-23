using global::ShippingManagementSystem.Domain.DTOs.DashboardDTOs;
using Swashbuckle.AspNetCore.Filters;
namespace ShippingManagementSystem.Web.Swagger.DashboardExamples.Responses.Success
{

    public class DashboardSummarySuccessExample : IExamplesProvider<DashboardSummaryDTO>
    {
        public DashboardSummaryDTO GetExamples()
        {
            return new DashboardSummaryDTO
            {
                AssignedOrders = 570,
                CompletedOrders = 450,
                EfficiencyRate = 78.95m,
                ChangeAssigned = 50,
                ChangeCompleted = 80,
                ChangePending = -30,
                ChangeEfficiency = 2.1m,
                TotalRevenue = 125000.50m,
                AverageWeight = 4.5m,
                OrdersByState = new Dictionary<string, int> { { "Delivered", 450 }, { "New", 50 } },
                MonthlyPerformance = new Dictionary<string, MonthlyPerformanceData>
                    {
                        { "1", new MonthlyPerformanceData { Value = 8.5m } },
                        { "9", new MonthlyPerformanceData { Value = 12.0m } }
                    }
            };
        }

    }
}
