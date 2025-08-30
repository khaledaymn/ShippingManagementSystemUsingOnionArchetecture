using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDtos
{
    public class AddMerchantDTO
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public float RejectedOrderPrecentage { get; set; }
        public int? SpecialPickUp { get; set; }
        public List<int>? BranchesIds { get; set; } = new();
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
    }
}

