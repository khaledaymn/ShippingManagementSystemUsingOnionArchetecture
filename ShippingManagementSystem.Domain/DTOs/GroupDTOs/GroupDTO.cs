using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string ? Name { get; set; } = string.Empty;
        public string ? CreationDate { get; set; }
        
    }
} 