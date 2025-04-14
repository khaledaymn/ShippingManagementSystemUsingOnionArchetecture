using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.StandardDTOs
{
    public class StandardDTO
    {
        public int Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Standard weight must be greater than 0")]
        public int StandardWeight { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Village price must be a positive value")]
        public int VillagePrice { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "KG price must be a positive value")]
        public int KGprice { get; set; }
        
        public bool IsDeleted { get; set; }
    }
} 