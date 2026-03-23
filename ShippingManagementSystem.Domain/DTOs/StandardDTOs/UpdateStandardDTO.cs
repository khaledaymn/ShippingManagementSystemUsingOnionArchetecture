using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.StandardDTOs
{
    public class UpdateStandardDTO
    {
        /// <summary> New threshold weight (KG). Leave null to keep current value. </summary>
        /// <example>7</example>
        public int? StandardWeight { get; set; }

        /// <summary> New additional fee for village deliveries. Leave null to keep current value. </summary>
        /// <example>25</example>
        public int? VillagePrice { get; set; }

        /// <summary> New price per extra KG. Leave null to keep current value. </summary>
        /// <example>15</example>
        public int? KGprice { get; set; }
    }
} 