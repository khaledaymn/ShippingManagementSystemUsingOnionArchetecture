using System;
using System.Collections.Generic;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class MerchantDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
        public double RejectedOrderPercentage { get; set; }
        public int? SpecialPickUp { get; set; }
        public DateTime HiringDate { get; set; }
        public List<string> MerchantCities { get; set; } = new List<string>();
        public List<MerchantSpecialPriceDTO> SpecialPrices { get; set; } = new List<MerchantSpecialPriceDTO>();
    }
}