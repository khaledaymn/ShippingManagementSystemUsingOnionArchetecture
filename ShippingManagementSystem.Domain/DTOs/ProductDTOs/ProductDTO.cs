using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Weight is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        public int Weight { get; set; }
        
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        
        public int OrderId { get; set; }
    }
} 