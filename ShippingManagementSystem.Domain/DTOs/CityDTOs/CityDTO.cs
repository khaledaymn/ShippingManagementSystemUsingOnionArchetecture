using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class CityDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Charge price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Charge price must be a positive value")]
        public double ChargePrice { get; set; }
        
        [Required(ErrorMessage = "Pickup price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Pickup price must be a positive value")]
        public double PickUpPrice { get; set; }
        
        [Required(ErrorMessage = "Governorate ID is required")]
        public int GovernorateId { get; set; }
        
        public string? GovernorateName { get; set; }
        
        public bool IsDeleted { get; set; }
    }
} 