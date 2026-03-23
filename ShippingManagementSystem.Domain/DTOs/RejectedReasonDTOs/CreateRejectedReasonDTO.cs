using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs
{
    public class CreateRejectedReasonDTO
    {
        /// <summary> The clear, concise text for the rejection reason. </summary>
        /// <example>Incorrect address provided</example>
        [Required(ErrorMessage = "Rejected reason text is required")]
        [StringLength(500, ErrorMessage = "Rejected reason text cannot exceed 500 characters")]
        public string Text { get; set; } = string.Empty;
    }
} 