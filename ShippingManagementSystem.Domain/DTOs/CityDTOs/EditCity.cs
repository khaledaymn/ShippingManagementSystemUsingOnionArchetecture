using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class EditCity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Charge price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Charge price must be a positive value")]
        public double ChargePrice { get; set; }

        [Required(ErrorMessage = "Pickup price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Pickup price must be a positive value")]
        public double PickUpPrice { get; set; }

        [Required(ErrorMessage = "Governorate ID is required")]
        public int GovernorateId { get; set; }
    }
}
