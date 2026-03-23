using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        /// <summary> Full name of the customer receiving the shipment. </summary>
        /// <example>Khaled Ayman</example>
        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; } = string.Empty;

        /// <summary> Primary contact phone number for the customer. </summary>
        /// <example>01098684485</example>
        [Required(ErrorMessage = "Customer phone is required")]
        [Phone]
        public string CustomerPhone1 { get; set; } = string.Empty;

        /// <summary> Secondary contact phone number (Optional). </summary>
        /// <example>01234567890</example>
        [Phone]
        public string? CustomerPhone2 { get; set; }

        /// <summary> Detailed address including village, street, and building number. Required if 'ShippingToVillage' is true. </summary>
        /// <example>Nile Street, Apartment 5, Menofia</example>
        public string? VillageAndStreet { get; set; } = string.Empty;

        /// <summary> Additional delivery instructions or merchant remarks. </summary>
        /// <example>Fragile items, please handle with care and call before arrival.</example>
        public string? Notes { get; set; }

        /// <summary> Total price of the products in the order (excluding shipping fees). </summary>
        /// <example>1500.00</example>
        [Required(ErrorMessage = "Order price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Order price must be a positive value")]
        public double OrderPrice { get; set; }

        /// <summary> Indicates if the destination is a rural/village area (Used for pricing logic). </summary>
        /// <example>false</example>
        public bool ShippingToVillage { get; set; }

        /// <summary> Foreign key for the destination City. </summary>
        /// <example>5</example>
        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }

        /// <summary> Foreign key for the shipping speed/type (e.g., Normal, Express). </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Charge type ID is required")]
        public int ChargeTypeId { get; set; }

        /// <summary> Foreign key for the branch handling the dispatch. </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Branch ID is required")]
        public int BranchId { get; set; }

        /// <summary> Foreign key for the merchant owning this order. </summary>
        /// <example>usr_merch_77</example>
        [Required(ErrorMessage = "Merchant ID is required")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary> Type of handling: 'DeliveryAtBranch' or 'PickupFromTheMerchant'. </summary>
        /// <example>PickupFromTheMerchant</example>
        public string OrderType { get; set; } = "DeliveryAtBranch";

        /// <summary> Method of settlement: 'CashOnDelivery', 'PaidInAdvance', or 'ExchangeOrder'. </summary>
        /// <example>CashOnDelivery</example>
        public string PaymentType { get; set; } = "CashOnDelivery";

        /// <summary> List of products included in the shipment. At least one product is mandatory. </summary>
        [Required(ErrorMessage = "At least one product is required")]
        [MinLength(1, ErrorMessage = "At least one product is required")]
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
} 