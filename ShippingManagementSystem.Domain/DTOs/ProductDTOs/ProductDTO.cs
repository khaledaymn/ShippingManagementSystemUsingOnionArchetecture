using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        /// <summary> Database identifier (Leave 0 for new orders). </summary>
        /// <example>0</example>
        public int Id { get; set; }

        /// <summary> Name or description of the product. </summary>
        /// <example>Wireless Headphones</example>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary> Individual product weight in grams. </summary>
        /// <example>350</example>
        [Required]
        [Range(1, int.MaxValue)]
        public int Weight { get; set; }

        /// <summary> Number of units of this product. </summary>
        /// <example>2</example>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary> The ID of the parent order. </summary>
        /// <example>1025</example>
        public int OrderId { get; set; }
    }
} 