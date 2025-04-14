using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class UpdateMerchantDTO
    {
        [Required]
        public string Id { get; set; }
        
        public string? Name { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public string? StoreName { get; set; }
        
        [Range(0, 100)]
        public double? RejectedOrderPercentage { get; set; }
        
        public int? SpecialPickUp { get; set; }
        
        public List<int>? CityIds { get; set; }
        
        public List<MerchantSpecialPriceDTO>? SpecialPrices { get; set; }
    }
}