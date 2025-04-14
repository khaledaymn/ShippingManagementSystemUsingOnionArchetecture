using ShippingManagementSystem.Domain.DTOs.BranchDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.EmployeeDTOs
{
    public class EmployeeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CreationDate { get; set; }
        public List<BranchDTO> Branches { get; set; }
        public string Permission { get; set; }
        public bool IsDeleted { get; set; }
    }
}
