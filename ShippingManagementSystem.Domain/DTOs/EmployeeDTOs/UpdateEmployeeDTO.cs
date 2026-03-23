using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.EmployeeDTOs
{
    public class UpdateEmployeeDTO
    {
        /// <summary> The ID of the employee to be updated. (Required) </summary>
        /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
        public string Id { get; set; }

        /// <summary> Updated full name. </summary>
        public string? Name { get; set; }

        /// <summary> Updated contact email. </summary>
        public string? Email { get; set; }

        /// <summary> Updated contact phone number. </summary>
        public string? PhoneNumber { get; set; }

        /// <summary> Updated physical address. </summary>
        public string? Address { get; set; }

        /// <summary> Updated list of Branch IDs. (Replaces current associations). </summary>
        public List<int>? BranchIds { get; set; }

        /// <summary> Updated Permission Group ID. (Re-calculates User Claims). </summary>
        public int? GroupId { get; set; }
    }
}
