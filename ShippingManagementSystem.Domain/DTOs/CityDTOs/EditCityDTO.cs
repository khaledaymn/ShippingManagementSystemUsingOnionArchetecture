using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class EditCityDTO
    {
        /// <summary> The ID of the city to be updated. </summary>
        /// <example>10</example>
        [Required]
        public int Id { get; set; }

        /// <summary> Updated city name. </summary>
        /// <example>New Nasr City</example>
        public string? Name { get; set; } = string.Empty;

        /// <summary> Updated shipping fee. </summary>
        /// <example>55.0</example>
        public double? ChargePrice { get; set; }

        /// <summary> Updated pickup fee. </summary>
        /// <example>20.0</example>
        public double? PickUpPrice { get; set; }

        /// <summary> Move city to a different Governorate (Optional). </summary>
        /// <example>2</example>
        public int? GovernorateId { get; set; }
    }
}
