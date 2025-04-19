using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.ChargeTypeDTOs
{
    public class ChargeTypeDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public double? ExtraPrice { get; set; }
        public int? NumOfDay { get; set; }
        public bool IsDeleted { get; set; }
    }
} 