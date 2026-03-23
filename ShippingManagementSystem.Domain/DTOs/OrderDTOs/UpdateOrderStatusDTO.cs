using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class UpdateOrderStatusDTO
    {
        /// <summary> The new status of the order (e.g., 'Delivered', 'PostPoned'). </summary>
        /// <example>Delivered</example>
        [Required]
        public string OrderState { get; set; } = string.Empty;

        /// <summary> Optional remarks for the status change. </summary>
        /// <example>Customer received and paid in full</example>
        public string? Notes { get; set; } = string.Empty;
    }
}