using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using ShippingManagementSystem.Domain.UserTypes;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.MerchantDtos
{
    public class MerchantDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? StartWorkDate { get; set; }       
        public string Email { get; set; }
        public int? SpecialPickUp { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
        public double RejectedOrderPrecentage { get; set; }
        public List<GetBranchDTO> Branches { get; set; } = new();
        public List<SpecialDeliveryPriceDTO>? SpecialDeliveryPrices { get; set; } = new();
    }
}
