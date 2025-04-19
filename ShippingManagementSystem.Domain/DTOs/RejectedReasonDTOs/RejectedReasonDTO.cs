using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.RejectedReasonDTOs
{
    public class RejectedReasonDTO
    {
        public int Id { get; set; }
        public string ? Text { get; set; } = string.Empty;
        public bool ?  IsDeleted { get; set; }
    }
} 