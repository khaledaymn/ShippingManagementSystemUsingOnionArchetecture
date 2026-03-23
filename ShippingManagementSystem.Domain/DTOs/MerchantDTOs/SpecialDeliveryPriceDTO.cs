namespace ShippingManagementSystem.Domain.DTOs
{
    public class SpecialDeliveryPriceDTO
    {
        /// <summary> The unique ID of the city. </summary>
        /// <example>5</example>
        public int cityId { get; set; }

        /// <summary> The name of the city (Read-only). </summary>
        /// <example>Alexandria</example>
        public string? CityName { get; set; }

        /// <summary> The override price for shipping to this city. </summary>
        /// <example>35.5</example>
        public double SpecialPrice { get; set; }
    }
}
