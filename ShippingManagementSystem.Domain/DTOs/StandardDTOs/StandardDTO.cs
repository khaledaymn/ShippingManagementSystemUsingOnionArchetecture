using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.StandardDTOs
{
    public class StandardDTO
    {
        /// <summary> Unique identifier for the standard configuration. </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> 
        /// The threshold weight (in KG) covered by the base shipping price. 
        /// Any weight beyond this will incur extra charges.
        /// </summary>
        /// <example>5</example>
        [Range(1, int.MaxValue, ErrorMessage = "Standard weight must be greater than 0")]
        public int StandardWeight { get; set; }

        /// <summary> 
        /// Fixed additional fee applied when shipping to remote villages or rural areas. 
        /// </summary>
        /// <example>20</example>
        [Range(0, double.MaxValue, ErrorMessage = "Village price must be a positive value")]
        public int VillagePrice { get; set; }

        /// <summary> 
        /// The cost charged for every single kilogram exceeding the <see cref="StandardWeight"/>. 
        /// </summary>
        /// <example>10</example>
        [Range(0, double.MaxValue, ErrorMessage = "KG price must be a positive value")]
        public int KGprice { get; set; }

        /// <summary> Status flag indicating if this configuration is deprecated. </summary>
        public bool IsDeleted { get; set; }
    }
} 