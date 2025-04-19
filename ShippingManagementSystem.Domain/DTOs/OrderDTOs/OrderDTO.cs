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
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone1 { get; set; } = string.Empty;
        public string? CustomerPhone2 { get; set; }
        public string VillageAndStreet { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string OrderState { get; set; } = "New";
        public string OrderType { get; set; } = "DeliveryAtBranch";
        public string PaymentType { get; set; } = "CashOnDelivery";
        public double ChargePrice { get; set; }
        public double OrderPrice { get; set; }
        public double AmountReceived { get; set; }
        public int TotalWeight { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsShippingToVillage { get; set; }
        public string? CityName { get; set; }
        public string? ChargeTypeName { get; set; }
        public string? BranchName { get; set; }
        public string? MerchantName { get; set; }
        public string? ShippigRepresentativeName { get; set; }
        
        public List<ProductDTO>? Products { get; set; }
    }
} 