namespace ShippingManagementSystem.Domain.Specifications.OrderSpecification
{
    public class OrderParams
    {
        /// <summary> Search term (Customer Name, Phone1, or Phone2). </summary>
        /// <example>Khaled</example>
        public string? Search { get; set; }

        /// <summary> Filter by shipment status (e.g., 'New', 'Delivered'). </summary>
        /// <example>New</example>
        public string? OrderState { get; set; }

        /// <summary> Filter by destination City ID. </summary>
        /// <example>5</example>
        public int? CityId { get; set; }

        /// <summary> Filter by the Branch ID processing the order. </summary>
        /// <example>1</example>
        public int? BranchId { get; set; }

        /// <summary> Filter by Merchant ID. </summary>
        /// <example>merch_123</example>
        public string? MerchantId { get; set; }

        /// <summary> Filter by assigned Shipping Representative (Driver) ID. </summary>
        /// <example>rep_456</example>
        public string? ShippingRepresentativeId { get; set; }

        /// <summary> Start date for creation filter. </summary>
        /// <example>2025-01-01</example>
        public DateTime? FromDate { get; set; }

        /// <summary> End date for creation filter. </summary>
        /// <example>2025-12-31</example>
        public DateTime? ToDate { get; set; }

        /// <summary> Filter by delivery type ('DeliveryAtBranch' or 'PickupFromTheMerchant'). </summary>
        /// <example>PickupFromTheMerchant</example>
        public string? OrderType { get; set; }

        /// <summary> Filter by payment method ('CashOnDelivery', 'ExchangeOrder', etc.). </summary>
        /// <example>CashOnDelivery</example>
        public string? PaymentType { get; set; }

        /// <summary> Filter by soft-deleted status. </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }

        /// <summary> Sort expression (e.g., 'date_desc', 'customer_asc', 'price_desc'). </summary>
        /// <example>date_desc</example>
        public string? Sort { get; set; }

        /// <summary> Page number for results. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of results per page (Max 100). </summary>
        /// <example>20</example>
        public int PageSize { get; set; } = 100;
    }
} 