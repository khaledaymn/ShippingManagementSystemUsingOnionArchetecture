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
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public List<Permission>? Permissions { get; set; }
    }
}
