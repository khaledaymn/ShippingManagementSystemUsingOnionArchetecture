using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.CityDTOs
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double ChargePrice { get; set; }
        public double PickUpPrice { get; set; }
        public string? GovernorateName { get; set; }
        public bool IsDeleted { get; set; }
    }
} 