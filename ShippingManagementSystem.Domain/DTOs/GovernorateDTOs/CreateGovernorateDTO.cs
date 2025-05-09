using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GovernorateDTOs
{
    public class CreateGovernorateDTO
    {
        [Required(ErrorMessage = "Governorate name is required")]
        [StringLength(100, ErrorMessage = "Governorate name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
} 