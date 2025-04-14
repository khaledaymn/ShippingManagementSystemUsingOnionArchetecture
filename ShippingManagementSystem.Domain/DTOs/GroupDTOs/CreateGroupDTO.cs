using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class CreateGroupDTO
    {
        [Required(ErrorMessage = "Group name is required")]
        [StringLength(100, ErrorMessage = "Group name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; }
    }
} 