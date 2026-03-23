using ShippingManagementSystem.Application.UserTypes.Enums;

namespace ShippingManagementSystem.Domain.DTOs.ShippingRepresentativeDTOs
{
    public class ShippingRepresentativeDTO
    {
        /// <summary> Unique identifier (Identity User ID). </summary>
        /// <example>rep_uuid_99</example>
        public string Id { get; set; }

        /// <summary> Full name. </summary>
        /// <example>Mahmoud El-Sayed</example>
        public string Name { get; set; }

        /// <summary> Registered email. </summary>
        /// <example>mahmoud.rep@shipping.com</example>
        public string Email { get; set; }

        /// <summary> Contact number. </summary>
        /// <example>01022334455</example>
        public string PhoneNumber { get; set; }

        /// <summary> Registered address. </summary>
        /// <example>45 Al-Horreya St, Menofia</example>
        public string Address { get; set; }

        /// <summary> Current discount/commission type. </summary>
        /// <example>Fixed</example>
        public DiscountType DiscountType { get; set; }

        /// <summary> Commission value or percentage. </summary>
        /// <example>10.5</example>
        public double CompanyPercentage { get; set; }

        /// <summary> Date the representative joined the fleet. </summary>
        /// <example>2026-03-21T08:00:00</example>
        public DateTime HiringDate { get; set; }

        /// <summary> List of names of governorates this representative covers. </summary>
        /// <example>["Cairo", "Giza", "Menofia"]</example>
        public List<string> Governorates { get; set; } = new List<string>();

        /// <summary> Activation status (True if soft-deleted). </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }
    }
}