using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class GroupDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Group name is required")]
        [StringLength(100, ErrorMessage = "Group name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public string CreationDate { get; set; }
        
    }
} 