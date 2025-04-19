using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.BranchDTOs
{
    public class GetBranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Location { get; set; } = string.Empty;
        public string CityName { get; set; }
    }
}
