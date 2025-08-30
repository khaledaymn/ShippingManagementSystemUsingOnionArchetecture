using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class GroupPermissionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Date { get; set; }
        public Dictionary<string, List<int>> Permissions { get; set; }
    }
}
