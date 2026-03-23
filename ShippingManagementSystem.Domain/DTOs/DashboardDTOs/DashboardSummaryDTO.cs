namespace ShippingManagementSystem.Domain.DTOs.DashboardDTOs
{
    public class DashboardSummaryDTO
    {
        /// <summary> Total number of orders assigned to the specific user/context this month. </summary>
        /// <example>150</example>
        public int AssignedOrders { get; set; }
        /// <summary> Number of orders successfully reached 'Delivered' state this month. </summary>
        /// <example>120</example>
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }

        /// <summary> Number of active orders currently in transit or awaiting action (Pending/Representative). </summary>
        /// <example>30</example>
        /// <summary> Success rate calculated as (Completed / Total) * 100. </summary>
        /// <example>80.0</example>
        public decimal EfficiencyRate { get; set; }

        /// <summary> Numerical difference in assigned orders compared to the previous month. </summary>
        /// <example>+15</example>
        public int ChangeAssigned { get; set; }

        /// <summary> Numerical difference in completed orders compared to the previous month. </summary>
        /// <example>+10</example>
        public int ChangeCompleted { get; set; }

        /// <summary> Numerical difference in pending orders compared to the previous month. </summary>
        /// <example>-5</example>
        public int ChangePending { get; set; }

        /// <summary> Percentage shift in efficiency compared to the previous month. </summary>
        /// <example>2.5</example>
        public decimal ChangeEfficiency { get; set; }
        // <summary> Grand total of all orders currently processed by the service. </summary>
        public int TotalOrders { get; set; }

        /// <summary> Aggregate monetary value received from delivered shipments. </summary>
        /// <example>45000.75</example>
        public decimal TotalRevenue { get; set; }

        /// <summary> The average weight per shipment in this period. </summary>
        /// <example>3.2</example>
        public decimal AverageWeight { get; set; }

        /// <summary> The average service charge price calculated across the current data set. </summary>
        /// <example>55.0</example>
        public decimal AverageChargePrice { get; set; }
        /// <summary> 
    /// Breakdown of orders by their lifecycle state. 
    /// Keys correspond to 'OrderState' Enum names. Used for Donut/Bar charts.
    /// </summary>
        public Dictionary<string, int> OrdersByState { get; set; } = new Dictionary<string, int>
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

        /// <summary> 
        /// Breakdown of orders by payment method (e.g., Cash, Advance). 
        /// Used for payment analysis tables.
        /// </summary>
        public Dictionary<string, int> OrdersByPaymentType { get; set; } = new Dictionary<string, int>
        {
            { "CashOnDelivery", 0 },
            { "PaidInAdvance", 0 },
            { "ExchangeOrder", 0 }
        };

        /// <summary> 
        /// Performance snapshots for each month of the current year (1-12). 
        /// Represents the percentage of total orders contributed by each month.
        /// </summary>
        public Dictionary<string, MonthlyPerformanceData> MonthlyPerformance { get; set; } = new Dictionary<string, MonthlyPerformanceData>();

        /// <summary> The mathematical sum of all orders categorized in the status distribution chart. </summary>
        public int OrderStatusDistributionTotal { get; set; } // Total for donut chart (e.g., 570)
    }

    public class MonthlyPerformanceData
    {
        /// <summary> The calculated percentage value (0-100) for a specific month. </summary>
        /// <example>12.5</example>
        public decimal Value { get; set; }
    }
}
