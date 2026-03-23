using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class CreateCityDTO
    {
        /// <summary> The unique name of the city. </summary>
        /// <example>Nasr City</example>
        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary> Initial shipping fee for this city. Must be positive. </summary>
        /// <example>45.0</example>
        [Required(ErrorMessage = "Charge price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Charge price must be a positive value")]
        public double ChargePrice { get; set; }

        /// <summary> Initial pickup fee for this city. Must be positive. </summary>
        /// <example>15.0</example>
        [Required(ErrorMessage = "Pickup price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Pickup price must be a positive value")]
        public double PickUpPrice { get; set; }

        /// <summary> The ID of the parent Governorate this city belongs to. </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Governorate ID is required")]
        public int GovernorateId { get; set; }
    }
} 