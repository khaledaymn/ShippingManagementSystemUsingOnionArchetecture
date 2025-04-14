using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MeduleDTOs
{
    public class MeduleDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Module name is required")]
        [StringLength(100, ErrorMessage = "Module name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
} 