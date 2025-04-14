using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Customer phone is required")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number format")]
        public string CustomerPhone1 { get; set; } = string.Empty;
        
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number format")]
        public string? CustomerPhone2 { get; set; }
        
        [Required(ErrorMessage = "Village and street details are required")]
        public string VillageAndStreet { get; set; } = string.Empty;
        
        public string? Notes { get; set; }
        
        [Required(ErrorMessage = "Order price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Order price must be a positive value")]
        public double OrderPrice { get; set; }
        
        public bool ShippingToVillage { get; set; }
        
        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }
        
        [Required(ErrorMessage = "Charge type ID is required")]
        public int ChargeTypeId { get; set; }
        
        [Required(ErrorMessage = "Branch ID is required")]
        public int BranchId { get; set; }
        
        [Required(ErrorMessage = "Merchant ID is required")]
        public string MerchantId { get; set; } = string.Empty;
        
        public string OrderType { get; set; } = "DeliveryAtBranch";
        
        public string PaymentType { get; set; } = "CashOnDelivery";
        
        [Required(ErrorMessage = "At least one product is required")]
        [MinLength(1, ErrorMessage = "At least one product is required")]
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
} 