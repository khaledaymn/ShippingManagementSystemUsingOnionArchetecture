using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShippingManagementSystem.Domain.DTOs.ProductDTOs;
using ShippingManagementSystem.Domain.Enums;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CreationDate { get; set; }
        
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
        public double ChargePrice { get; set; }
        public string OrderState { get; set; } = "New";
        public string OrderType { get; set; } = "DeliveryAtBranch";
        public string PaymentType { get; set; } = "CashOnDelivery";
        public double OrderPrice { get; set; }
        public int TotalWeight { get; set; }
        public bool IsDeleted { get; set; }
        public bool ShippingToVillage { get; set; }
        public double AmountReceived { get; set; }
        
        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }
        public string? CityName { get; set; }
        
        [Required(ErrorMessage = "Charge type ID is required")]
        public int ChargeTypeId { get; set; }
        public string? ChargeTypeName { get; set; }
        
        [Required(ErrorMessage = "Branch ID is required")]
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        
        [Required(ErrorMessage = "Merchant ID is required")]
        public string MerchantId { get; set; } = string.Empty;
        public string? MerchantName { get; set; }
        
        public string? ShippigRepresentativeId { get; set; }
        public string? ShippigRepresentativeName { get; set; }
        
        public List<ProductDTO>? Products { get; set; }
    }
} 