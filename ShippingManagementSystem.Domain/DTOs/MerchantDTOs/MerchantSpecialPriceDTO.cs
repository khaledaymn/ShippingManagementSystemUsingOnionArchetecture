using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class MerchantSpecialPriceDTO
    {
        [Required]
        public int CityId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Special price must be a positive value")]
        public double SpecialPrice { get; set; }
    }
}