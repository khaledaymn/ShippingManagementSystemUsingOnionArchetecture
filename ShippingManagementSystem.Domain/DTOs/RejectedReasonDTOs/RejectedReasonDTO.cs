using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs
{
    public class RejectedReasonDTO
    {
        /// <summary> Unique identifier for the rejection reason. </summary>
        /// <example>101</example>
        public int Id { get; set; }

        /// <summary> The descriptive text explaining why the order was rejected. </summary>
        /// <example>Customer refused to receive the package</example>
        public string? Text { get; set; } = string.Empty;

        /// <summary> Soft-delete status flag. </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }
    }
} 