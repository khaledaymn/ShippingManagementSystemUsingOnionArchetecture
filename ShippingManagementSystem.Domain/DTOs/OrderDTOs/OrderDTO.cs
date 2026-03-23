using ShippingManagementSystem.Domain.DTOs.ProductDTOs;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        /// <summary> Unique order identifier. </summary>
        /// <example>1025</example>
        public int Id { get; set; }

        /// <summary> Formatted creation date and time. </summary>
        /// <example>2025-08-11 14:30:00</example>
        public string CreationDate { get; set; }

        /// <summary> Recipient's full name. </summary>
        /// <example>Omar Ahmed</example>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary> Primary contact number for the customer. </summary>
        /// <example>01099887766</example>
        public string CustomerPhone1 { get; set; } = string.Empty;

        /// <summary> Secondary contact number (Optional). </summary>
        /// <example>01122334455</example>
        public string? CustomerPhone2 { get; set; }

        /// <summary> Specific delivery address details (Village/Street). </summary>
        /// <example>Nile Street, Building 10</example>
        public string VillageAndStreet { get; set; } = string.Empty;

        /// <summary> Special instructions or notes. </summary>
        /// <example>Call before arrival</example>
        public string? Notes { get; set; }

        /// <summary> Current status of the shipment. </summary>
        /// <example>Pendding</example>
        public string OrderState { get; set; } = "New";

        /// <summary> Handling method. </summary>
        /// <example>DeliveryAtBranch</example>
        public string OrderType { get; set; } = "DeliveryAtBranch";

        /// <summary> Financial settlement method. </summary>
        /// <example>CashOnDelivery</example>
        public string PaymentType { get; set; } = "CashOnDelivery";

        /// <summary> Calculated shipping fee. </summary>
        /// <example>45.0</example>
        public double ChargePrice { get; set; }

        /// <summary> Value of the products inside the shipment. </summary>
        /// <example>500.0</example>
        public double OrderPrice { get; set; }

        /// <summary> Total cash collected upon delivery. </summary>
        /// <example>545.0</example>
        public double AmountReceived { get; set; }

        /// <summary> Cumulative weight of all products in grams/KG. </summary>
        /// <example>2500</example>
        public int TotalWeight { get; set; }

        /// <summary> Indicates if the order is soft-deleted. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }

        /// <summary> Indicates if the destination is a rural area. </summary>
        /// <example>false</example>
        public bool IsShippingToVillage { get; set; }

        /// <summary> Name of the destination city. </summary>
        /// <example>Cairo</example>
        public string? CityName { get; set; }

        /// <summary> Level of service (e.g., Normal, Express). </summary>
        /// <example>Express</example>
        public string? ChargeTypeName { get; set; }

        /// <summary> Branch handling the logistics. </summary>
        /// <example>Main Cairo Branch</example>
        public string? BranchName { get; set; }

        /// <summary> Name of the merchant who owns the order. </summary>
        /// <example>Tech Store Egypt</example>
        public string? MerchantName { get; set; }

        /// <summary> Name of the driver/rep assigned to deliver. </summary>
        /// <example>Mahmoud Delivery</example>
        public string? ShippingRepresentativeName { get; set; }

        /// <summary> List of items included in this shipment. </summary>
        public List<ProductDTO>? Products { get; set; }
    }
} 