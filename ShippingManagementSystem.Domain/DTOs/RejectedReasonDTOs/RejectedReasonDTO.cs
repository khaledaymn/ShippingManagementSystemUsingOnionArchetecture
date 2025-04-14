using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs
{
    public class RejectedReasonDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Rejected reason text is required")]
        [StringLength(500, ErrorMessage = "Rejected reason text cannot exceed 500 characters")]
        public string Text { get; set; } = string.Empty;
        
        public bool IsDeleted { get; set; }
    }
} 