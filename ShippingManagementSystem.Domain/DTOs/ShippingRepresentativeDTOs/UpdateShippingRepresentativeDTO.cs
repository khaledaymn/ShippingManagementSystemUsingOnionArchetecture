using ShippingManagementSystem.Application.UserTypes.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class UpdateShippingRepresentativeDTO
    {
        [Required]
        public string Id { get; set; }
        
        public string? Name { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public DiscountType? DiscountType { get; set; }
        
        [Range(0, 100)]
        public double? CompanyPercentage { get; set; }
        
        public List<int>? GovernorateIds { get; set; }
    }
}