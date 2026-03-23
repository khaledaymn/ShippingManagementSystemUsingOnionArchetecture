using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GovernorateDTOs
{
    public class GovernorateDTO
    {
        /// <summary> Unique identifier for the governorate. </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> The official name of the governorate. </summary>
        /// <example>Cairo</example>
        public string? Name { get; set; } = string.Empty;

        /// <summary> Indicates if the governorate is marked as deleted (Soft-deleted). </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }
    }
} 