using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs
{
    public class CreateRejectedReasonDTO
    {
        [Required(ErrorMessage = "Rejected reason text is required")]
        [StringLength(500, ErrorMessage = "Rejected reason text cannot exceed 500 characters")]
        public string Text { get; set; } = string.Empty;
    }
} 