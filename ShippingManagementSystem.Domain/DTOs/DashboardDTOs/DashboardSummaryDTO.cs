namespace ShippingManagementSystem.Domain.DTOs.DashboardDTOs
{
    public class DashboardSummaryDTO
    {
        // Monthly Statistics from stats-grid
        public int AssignedOrders { get; set; } // Total orders assigned to the user
        public int CompletedOrders { get; set; } // Orders with state "Delivered"
        public int PendingOrders { get; set; } // Sum of states like "Pendding", "DeliveredToTheRepresentative", etc.
        public decimal EfficiencyRate { get; set; } // Percentage of completed orders
        public int ChangeAssigned { get; set; } // Change in Assigned Orders (e.g., +50)
        public int ChangeCompleted { get; set; } // Change in Completed Orders (e.g., +80)
        public int ChangePending { get; set; } // Change in Pending Orders (e.g., -30)
        public decimal ChangeEfficiency { get; set; } // Change in Efficiency Rate (e.g., +2.1%)

        // Financial and General Stats
        public int TotalOrders { get; set; } // Total orders from the service
        public decimal TotalRevenue { get; set; } // Sum of AmountReceived
        public decimal AverageWeight { get; set; } // Average TotalWeight
        public decimal AverageChargePrice { get; set; } // Average ChargePrice

        // Order States Distribution (for table and donut chart)
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

        // Payment Types Distribution (for table)
        public Dictionary<string, int> OrdersByPaymentType { get; set; } = new Dictionary<string, int>
        {
            { "CashOnDelivery", 0 },
            { "PaidInAdvance", 0 },
            { "ExchangeOrder", 0 }
        };

        // Monthly Performance (for the current month, representing order state percentages)
        public Dictionary<string, MonthlyPerformanceData> MonthlyPerformance { get; set; } = new Dictionary<string, MonthlyPerformanceData>();

        // Order Status Distribution (for donut chart - simplified total)
        public int OrderStatusDistributionTotal { get; set; } // Total for donut chart (e.g., 570)
    }

    public class MonthlyPerformanceData
    {
        public decimal Value { get; set; } // Percentage value (0-100%)
    }
}
