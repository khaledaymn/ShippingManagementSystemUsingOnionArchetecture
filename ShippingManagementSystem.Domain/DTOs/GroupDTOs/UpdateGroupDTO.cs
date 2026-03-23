using ShippingManagementSystem.Domain.DTOs.MeduleDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class UpdateGroupDTO
    {
        /// <summary> ID of the group to be updated. </summary>
        public int Id { get; set; }

        /// <summary> New name for the group (optional). </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary> 
        /// New list of permissions. 
        /// Note: Providing this list will replace all existing permissions for this group.
        /// </summary>
        public List<Permission>? Permissions { get; set; }
    }
}
