using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class AddMerchantDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string StoreName { get; set; }
        
        [Required]
        [Range(0, 100)]
        public double RejectedOrderPercentage { get; set; }
        
        public int? SpecialPickUp { get; set; }
        
        public List<int> CityIds { get; set; } = new List<int>();
        
        public List<MerchantSpecialPriceDTO> SpecialPrices { get; set; } = new List<MerchantSpecialPriceDTO>();
    }
}