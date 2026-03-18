using Microsoft.EntityFrameworkCore;
using ShippingManagementSystem.Domain.DTOs.DashboardDTOs;
using ShippingManagementSystem.Domain.Enums;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Infrastructure.Data;

namespace ShippingManagementSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _dbContext;

        public DashboardService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(string? userId)
        {
            // Current date (today: September 03, 2025, 01:37 AM EEST = 10:37 PM UTC, September 02, 2025)
            var today = DateTime.UtcNow;
            var currentYear = today.Year;
            var currentMonthStart = new DateTime(today.Year, today.Month, 1); // Start of current month
            var previousMonthStart = currentMonthStart.AddMonths(-1); // Start of previous month
            var previousMonthEnd = currentMonthStart.AddDays(-1); // End of previous month

            // Base query for orders, excluding deleted ones
            var query = _dbContext.Orders
                .Where(o => !o.IsDeleted);

            // Filter by userId if provided (for Merchant or ShippingRepresentative)
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(o => o.MerchantId == userId || o.ShippingRepresentativeId == userId);
            }

            // Get total orders across all time (filtered by userId if provided, excluding deleted orders)
            var totalOrdersAllTime = await query.CountAsync();

            // Aggregate query for current month's data
            var currentData = await query
                .Where(o => o.CreationDate >= currentMonthStart && o.CreationDate < currentMonthStart.AddMonths(1))
                .ToListAsync(); // Fetch data to memory to avoid translation error

            // Initialize all possible order states
            var allOrderStates = new Dictionary<string, int>
            {
                { "New", 0 },
                { "Pendding", 0 },
                { "DeliveredToTheRepresentative", 0 },
                { "Delivered", 0 },
                { "CannotBeReached", 0 },
                { "PostPoned", 0 },
                { "PartiallyDelivered", 0 },
                { "CanceledByCustomer", 0 },
                { "RejectedWithPayment", 0 },
                { "RejectedWithPartialPayment", 0 },
                { "RejectedWithoutPayment", 0 }
            };

            // Initialize all possible payment types
            var allPaymentTypes = new Dictionary<string, int>
            {
                { "CashOnDelivery", 0 },
                { "PaidInAdvance", 0 },
                { "ExchangeOrder", 0 }
            };

            var currentSummary = currentData
                .GroupBy(o => 1)
                .Select(g => new
                {
                    TotalOrders = g.Count(),
                    OrdersByState = g.GroupBy(o => o.OrderState)
                        .AsEnumerable()
                        .ToDictionary(o => o.Key.ToString(), o => o.Count()),
                    OrdersByPaymentType = g.GroupBy(o => o.PaymentType)
                        .AsEnumerable()
                        .ToDictionary(o => o.Key.ToString(), o => o.Count()),
                    TotalRevenue = g.Sum(o => (decimal?)o.AmountReceived) ?? 0,
                    AverageWeight = g.Average(o => (decimal?)o.TotalWeight) ?? 0,
                    AverageChargePrice = g.Average(o => (decimal?)o.ChargePrice) ?? 0
                })
                .FirstOrDefault();

            // Merge currentSummary.OrdersByState with allOrderStates
            var ordersByState = allOrderStates;
            if (currentSummary?.OrdersByState != null)
            {
                foreach (var state in currentSummary.OrdersByState)
                {
                    if (ordersByState.ContainsKey(state.Key))
                    {
                        ordersByState[state.Key] = state.Value;
                    }
                }
            }

            // Merge currentSummary.OrdersByPaymentType with allPaymentTypes
            var ordersByPaymentType = allPaymentTypes;
            if (currentSummary?.OrdersByPaymentType != null)
            {
                foreach (var paymentType in currentSummary.OrdersByPaymentType)
                {
                    if (ordersByPaymentType.ContainsKey(paymentType.Key))
                    {
                        ordersByPaymentType[paymentType.Key] = paymentType.Value;
                    }
                }
            }

            // Aggregate query for previous month's data
            var previousData = await query
                .Where(o => o.CreationDate >= previousMonthStart && o.CreationDate <= previousMonthEnd)
                .ToListAsync(); // Fetch previous month's data to memory

            var previousSummary = previousData
                .GroupBy(o => 1)
                .Select(g => new
                {
                    TotalOrders = g.Count(),
                    CompletedOrders = g.Count(o => o.OrderState == OrderState.Delivered),
                    PendingOrders = g.Count(o => o.OrderState == OrderState.Pendding || o.OrderState == OrderState.DeliveredToTheRepresentative)
                })
                .FirstOrDefault();

            // Calculate MonthlyPerformance for all months (1 to 12) of the current year
            var monthlyPerformance = new Dictionary<string, MonthlyPerformanceData>();
            for (int month = 1; month <= 12; month++)
            {
                var monthStart = new DateTime(currentYear, month, 1);
                var monthEnd = monthStart.AddMonths(1);

                // Get order count for the specific month
                var monthOrderCount = await query
                    .Where(o => o.CreationDate >= monthStart && o.CreationDate < monthEnd)
                    .CountAsync();

                // Calculate percentage for the month
                var percentage = totalOrdersAllTime > 0 ? (decimal)monthOrderCount / totalOrdersAllTime * 100 : 0;
                monthlyPerformance[month.ToString()] = new MonthlyPerformanceData
                {
                    Value = Math.Round(percentage, 2)
                };
            }

            // Initialize DTO
            var dashboardDto = new DashboardSummaryDTO
            {
                TotalOrders = currentSummary?.TotalOrders ?? 0,
                TotalRevenue = currentSummary?.TotalRevenue ?? 0,
                AverageWeight = currentSummary?.AverageWeight ?? 0,
                AverageChargePrice = currentSummary?.AverageChargePrice ?? 0,
                OrdersByState = ordersByState,
                OrdersByPaymentType = ordersByPaymentType,
                MonthlyPerformance = monthlyPerformance
            };

            // Calculate Derived Statistics
            dashboardDto.AssignedOrders = dashboardDto.TotalOrders;
            dashboardDto.CompletedOrders = dashboardDto.OrdersByState.ContainsKey("Delivered") ? dashboardDto.OrdersByState["Delivered"] : 0;
            dashboardDto.PendingOrders = dashboardDto.OrdersByState.Where(s => new[] { "Pendding", "DeliveredToTheRepresentative" }.Contains(s.Key)).Sum(s => s.Value);
            dashboardDto.EfficiencyRate = dashboardDto.TotalOrders > 0 ? (decimal)dashboardDto.CompletedOrders / dashboardDto.TotalOrders * 100 : 0;

            // Calculate Changes based on previous month's data
            dashboardDto.ChangeAssigned = (currentSummary?.TotalOrders ?? 0) - (previousSummary?.TotalOrders ?? 0);
            dashboardDto.ChangeCompleted = (dashboardDto.CompletedOrders) - (previousSummary?.CompletedOrders ?? 0);
            dashboardDto.ChangePending = (dashboardDto.PendingOrders) - (previousSummary?.PendingOrders ?? 0);
            dashboardDto.ChangeEfficiency = dashboardDto.TotalOrders > 0 && (previousSummary?.TotalOrders ?? 0) > 0
                ? ((decimal)dashboardDto.CompletedOrders / dashboardDto.TotalOrders * 100) - ((decimal)(previousSummary?.CompletedOrders ?? 0) / (previousSummary?.TotalOrders ?? 1) * 100)
                : 0;

            dashboardDto.OrderStatusDistributionTotal = dashboardDto.OrdersByState.Values.Sum();

            return dashboardDto;
        }
    }
}
