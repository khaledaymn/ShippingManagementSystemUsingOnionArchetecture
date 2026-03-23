using System;
using System.ComponentModel.DataAnnotations;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class GroupDTO
    {
        /// <summary> Unique identifier for the permission group. </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> The descriptive name of the group (e.g., 'Accountants', 'Supervisors'). </summary>
        /// <example>Accountant Group</example>
        public string? Name { get; set; } = string.Empty;

        /// <summary> The date when the group was first created (Formatted as yyyy-MM-dd HH:mm). </summary>
        /// <example>2024-05-15 14:30</example>
        public string? CreationDate { get; set; }
    }
} 