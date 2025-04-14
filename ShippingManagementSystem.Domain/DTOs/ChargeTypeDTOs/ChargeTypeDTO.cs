using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs
{
    public class ChargeTypeDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Charge type name is required")]
        [StringLength(100, ErrorMessage = "Charge type name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Extra price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Extra price must be a positive value")]
        public double ExtraPrice { get; set; }
        
        [Required(ErrorMessage = "Number of days is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of days must be a positive value")]
        public int NumOfDay { get; set; }
        
        public bool IsDeleted { get; set; }
    }
} 