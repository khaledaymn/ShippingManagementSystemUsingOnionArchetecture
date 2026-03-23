using ShippingManagementSystem.Application.UserTypes.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class UpdateShippingRepresentativeDTO
    {
        /// <summary> The ID of the representative to be updated. </summary>
        /// <example>rep_uuid_99</example>
        [Required]
        public string Id { get; set; }

        /// <summary> Updated full name. </summary>
        /// <example>Mahmoud A. El-Sayed</example>
        public string? Name { get; set; }

        /// <summary> Updated email address. </summary>
        /// <example>new.mahmoud@shipping.com</example>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary> Updated phone number. </summary>
        /// <example>01055667788</example>
        public string? PhoneNumber { get; set; }

        /// <summary> Updated address. </summary>
        /// <example>New Address St, Tanta</example>
        public string? Address { get; set; }

        /// <summary> Change in discount type. </summary>
        /// <example>Percentage</example>
        public DiscountType? DiscountType { get; set; }

        /// <summary> Updated percentage or fixed fee. </summary>
        /// <example>12.0</example>
        [Range(0, 100)]
        public double? CompanyPercentage { get; set; }

        /// <summary> Updated list of governorate IDs (Replaces existing list). </summary>
        /// <example>[1, 2, 4]</example>
        public List<int>? GovernorateIds { get; set; }
    }
}