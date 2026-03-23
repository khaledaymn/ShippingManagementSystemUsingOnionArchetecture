using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs
{
    public class ChargeTypeDTO
    {
        /// <summary> Unique identifier for the charge type. </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> Descriptive name (e.g., 'Express Shipping', 'Standard'). </summary>
        /// <example>Express Delivery</example>
        public string? Name { get; set; } = string.Empty;

        /// <summary> The additional cost added to the base shipping fee. </summary>
        /// <example>25.0</example>
        public double? ExtraPrice { get; set; }

        /// <summary> Estimated number of days for delivery under this type. </summary>
        /// <example>2</example>
        public int? NumOfDay { get; set; }

        /// <summary> Indicates if the charge type is archived/deleted. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }
    }
} 