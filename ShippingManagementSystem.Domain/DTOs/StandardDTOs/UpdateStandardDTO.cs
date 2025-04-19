using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.StandardDTOs
{
    public class UpdateStandardDTO
    {
        public int? StandardWeight { get; set; }
        public int? VillagePrice { get; set; }
        public int? KGprice { get; set; }
    }
} 