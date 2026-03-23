using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class UpdateMerchantDTO
    {
        /// <summary> Unique identifier of the merchant to update. </summary>
        /// <example>usr_merch_001</example>
        [Required]
        public string Id { get; set; }

        /// <summary> Updated physical address. </summary>
        /// <example>456 New Street, Giza</example>
        public string? Address { get; set; }

        /// <summary> Updated full name. </summary>
        /// <example>Ahmed M. Merchant</example>
        public string? Name { get; set; }

        /// <summary> Updated email address. </summary>
        /// <example>new.email@example.com</example>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary> Updated phone number. </summary>
        /// <example>01099887766</example>
        [Phone]
        public string? PhoneNumber { get; set; }

        /// <summary> Updated store trade name. </summary>
        /// <example>Ayman Tech Store V2</example>
        public string? StoreName { get; set; }

        /// <summary> Updated rejection rate percentage. </summary>
        /// <example>1.8</example>
        public double? RejectedOrderPrecentage { get; set; }

        /// <summary> Updated custom pickup fee. </summary>
        /// <example>20</example>
        public int? SpecialPickUp { get; set; }

        /// <summary> Updated list of associated branch IDs. </summary>
        /// <example>[1, 3]</example>
        public List<int>? BranchesId { get; set; } = new();

        /// <summary> Updated custom delivery prices for specific cities. </summary>
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
    }
}
