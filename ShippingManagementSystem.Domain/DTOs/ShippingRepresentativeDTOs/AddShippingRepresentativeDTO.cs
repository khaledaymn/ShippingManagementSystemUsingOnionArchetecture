using ShippingManagementSystem.Application.UserTypes.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class AddShippingRepresentativeDTO
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
        public DiscountType DiscountType { get; set; }
        
        [Required]
        [Range(0, 100)]
        public double CompanyPercentage { get; set; }
        
        public List<int> GovernorateIds { get; set; } = new List<int>();
    }
}