using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.OrderDTOs
{
    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "Order status is required")]
        public string OrderState { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
    }
}