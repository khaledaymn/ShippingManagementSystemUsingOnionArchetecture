using ShippingManagementSystem.Application.UserTypes.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class AddShippingRepresentativeDTO
    {
        /// <summary> Full legal name of the representative. </summary>
        /// <example>Mahmoud El-Sayed</example>
        [Required]
        public string Name { get; set; }

        /// <summary> Business email for system access. </summary>
        /// <example>mahmoud.rep@shipping.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary> Contact phone number. </summary>
        /// <example>01022334455</example>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary> Security password for the representative's account. </summary>
        /// <example>Rep@Pass2026</example>
        [Required]
        public string Password { get; set; }

        /// <summary> The format of commission calculation (e.g., Fixed amount or Percentage). </summary>
        /// <example>Percentage</example>
        [Required]
        public DiscountType DiscountType { get; set; }

        /// <summary> The percentage or fixed value the company takes/gives per order. </summary>
        /// <example>10.5</example>
        [Required]
        [Range(0, 100)]
        public double CompanyPercentage { get; set; }

        /// <summary> Residential or office address. </summary>
        /// <example>45 Al-Horreya St, Menofia</example>
        [Required]
        public string Address { get; set; }

        /// <summary> List of IDs for governorates (regions) the representative is assigned to cover. </summary>
        /// <example>[1, 3, 5]</example>
        public List<int> GovernorateIds { get; set; } = new List<int>();
    }
}