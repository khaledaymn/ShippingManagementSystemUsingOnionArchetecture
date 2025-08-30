using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDTOs
{
    public class UpdateMerchantDTO
    {
        [Required]
        public string Id { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? StoreName { get; set; }
        public double? RejectedOrderPrecentage { get; set; }
        public int? SpecialPickUp { get; set; }
        public List<int>? BranchesId { get; set; } = new();
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
        
    }
}
