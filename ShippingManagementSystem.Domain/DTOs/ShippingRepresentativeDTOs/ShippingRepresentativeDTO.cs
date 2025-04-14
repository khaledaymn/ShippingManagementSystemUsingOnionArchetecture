using ShippingManagementSystem.Application.UserTypes.Enums;
using System;
using System.Collections.Generic;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class ShippingRepresentativeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DiscountType DiscountType { get; set; }
        public double CompanyPercentage { get; set; }
        public DateTime HiringDate { get; set; }
        public List<string> Governorates { get; set; } = new List<string>();
    }
}