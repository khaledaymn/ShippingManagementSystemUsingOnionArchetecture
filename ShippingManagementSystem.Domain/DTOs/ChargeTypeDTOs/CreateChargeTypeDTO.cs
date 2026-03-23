using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs
{
    public class CreateChargeTypeDTO
    {
        /// <summary> The unique name for the shipping speed. </summary>
        /// <example>Same Day Delivery</example>
        [Required(ErrorMessage = "Charge type name is required")]
        [StringLength(100, ErrorMessage = "Charge type name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary> The premium price added for this service. Must be 0 or more. </summary>
        /// <example>40.0</example>
        [Required(ErrorMessage = "Extra price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Extra price must be a positive value")]
        public double ExtraPrice { get; set; }

        /// <summary> Expected duration in days. Must be 0 or more. </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Number of days is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number of days must be a positive value")]
        public int NumOfDay { get; set; }
    }
} 