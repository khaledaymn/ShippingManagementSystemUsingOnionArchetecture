using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.StandardDTOs
{
    public class CreateStandardDTO
    {
        [Required(ErrorMessage = "Standard weight is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Standard weight must be greater than 0")]
        public int StandardWeight { get; set; }
        
        [Required(ErrorMessage = "Village price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Village price must be a positive value")]
        public int  VillagePrice { get; set; }
        
        [Required(ErrorMessage = "KG price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "KG price must be a positive value")]
        public int KGprice { get; set; }
    }
} 