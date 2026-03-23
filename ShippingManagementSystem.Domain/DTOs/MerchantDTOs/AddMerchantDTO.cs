using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDtos
{
    public class AddMerchantDTO
    {
        /// <summary> The merchant's physical or business address. </summary>
        /// <example>123 Merchant Lane, Cairo</example>
        [Required]
        public string Address { get; set; }

        /// <summary> Full legal name of the merchant. </summary>
        /// <example>Ahmed Merchant</example>
        [Required]
        public string Name { get; set; }

        /// <summary> Primary business email address. </summary>
        /// <example>ahmed.store@example.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary> Primary contact phone number. </summary>
        /// <example>01011223344</example>
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary> Initial password for the merchant account. </summary>
        /// <example>SecureP@ssw0rd2025</example>
        [Required]
        public string Password { get; set; }

        /// <summary> The trade name of the merchant's store. </summary>
        /// <example>Ayman Tech Store</example>
        [Required]
        public string StoreName { get; set; }

        /// <summary> Historical percentage of orders rejected by this merchant. </summary>
        /// <example>2.5</example>
        [Required]
        public float RejectedOrderPrecentage { get; set; }

        /// <summary> Optional customized pickup fee. If null, standard city rates apply. </summary>
        /// <example>15</example>
        public int? SpecialPickUp { get; set; }

        /// <summary> List of branch IDs that this merchant is associated with. </summary>
        /// <example>[1, 2]</example>
        public List<int>? BranchesIds { get; set; } = new();

        /// <summary> Custom delivery price overrides for specific cities. </summary>
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
    }
}

