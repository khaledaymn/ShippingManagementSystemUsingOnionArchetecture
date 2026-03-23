using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.EmployeeDTOs
{
    public class AddEmployeeDTO
    {
        /// <summary> Full name of the employee. </summary>
        /// <example>Ahmed Ali</example>
        public string Name { get; set; } = string.Empty;

        /// <summary> Official work email (Used for login). Must be unique. </summary>
        /// <example>ahmed.ali@company.com</example>
        public string Email { get; set; } = string.Empty;

        /// <summary> Primary contact phone number. </summary>
        /// <example>01012345678</example>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary> Physical residential address of the employee. </summary>
        /// <example>123 Nasr City, Cairo</example>
        public string Address { get; set; } = string.Empty;

        /// <summary> List of Branch IDs the employee is authorized to manage or view. </summary>
        /// <example>[1, 2]</example>
        public List<int> BranchIds { get; set; }

        /// <summary> The ID of the Permission Group (Role/Permissions set) assigned to this employee. </summary>
        /// <example>3</example>
        public int GroupId { get; set; }

        /// <summary> Secure login password. Must meet complexity requirements. </summary>
        /// <example>SecurePass@123</example>
        public string Password { get; set; } = string.Empty;
    }
}
