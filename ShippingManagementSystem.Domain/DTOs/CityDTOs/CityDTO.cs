using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class CityDTO
    {
        /// <summary> Unique identifier for the city. </summary>
        /// <example>10</example>
        public int Id { get; set; }

        /// <summary> Official name of the city. </summary>
        /// <example>6th of October</example>
        public string Name { get; set; } = string.Empty;

        /// <summary> The standard shipping fee (Charge) for this city. </summary>
        /// <example>50.0</example>
        public double ChargePrice { get; set; }

        /// <summary> The fee for picking up an order from this city. </summary>
        /// <example>10.0</example>
        public double PickUpPrice { get; set; }

        /// <summary> The name of the parent governorate. </summary>
        /// <example>Giza</example>
        public string? GovernorateName { get; set; }

        /// <summary> Indicates if the city is marked as deleted. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }
    }
} 