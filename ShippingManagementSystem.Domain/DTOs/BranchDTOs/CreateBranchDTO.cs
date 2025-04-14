using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class CreateBranchDTO
    {
        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }
    }
} 