using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class CreateGroupDTO
    {
        /// <summary> Unique name for the group. </summary>
        /// <example>Regional Managers</example>
        [Required(ErrorMessage = "Group name is required")]
        [StringLength(100, ErrorMessage = "Group name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary> List of modules and their respective permission values (Create, View, Edit, etc.). </summary>
        public List<Permission> Permissions { get; set; }
    }
} 