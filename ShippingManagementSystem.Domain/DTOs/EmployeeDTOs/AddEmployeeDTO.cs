using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.EmployeeDTOs
{
    public class AddEmployeeDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<int> BranchIds { get; set; }
        public int GroupId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
