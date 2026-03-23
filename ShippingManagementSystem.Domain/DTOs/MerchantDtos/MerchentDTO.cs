using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.UserTypes;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDtos
{
    public class MerchantDTO
    {
        /// <summary> Unique merchant identifier (GUID). </summary>
        /// <example>usr_merch_001</example>
        public string Id { get; set; }

        /// <summary> Full legal name. </summary>
        /// <example>Ahmed Merchant</example>
        public string Name { get; set; }

        /// <summary> Registered business address. </summary>
        /// <example>123 Merchant Lane, Cairo</example>
        public string Address { get; set; }

        /// <summary> Soft-delete status flag. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }

        /// <summary> The date the merchant started working with the system. </summary>
        /// <example>2024-03-21T00:00:00</example>
        public DateTime? StartWorkDate { get; set; }

        /// <summary> Registered email address. </summary>
        /// <example>ahmed.store@example.com</example>
        public string Email { get; set; }

        /// <summary> Customized pickup fee if applicable. </summary>
        /// <example>15</example>
        public int? SpecialPickUp { get; set; }

        /// <summary> Contact phone number. </summary>
        /// <example>01011223344</example>
        public string PhoneNumber { get; set; }

        /// <summary> Registered trade name. </summary>
        /// <example>Ayman Tech Store</example>
        public string StoreName { get; set; }

        /// <summary> Percentage of rejected orders. </summary>
        /// <example>2.5</example>
        public double RejectedOrderPrecentage { get; set; }

        /// <summary> Detailed list of branches the merchant can operate through. </summary>
        public List<GetBranchDTO> Branches { get; set; } = new();

        /// <summary> List of custom delivery prices for specific cities. </summary>
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
    }
}
