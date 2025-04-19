using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GovernorateDTOs
{
    public class GovernorateDTO
    {
        public int Id { get; set; }
        public string ? Name { get; set; } = string.Empty;
        public bool ? IsDeleted { get; set; }
    }
} 