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
        /// <summary> The unique Identity User ID (GUID). </summary>
        /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
        public string Id { get; set; }

        /// <summary> Display name of the employee. </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> Contact email address. </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary> Contact phone number. </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary> The date the employee was hired or added (Formatted as yyyy-MM-dd). </summary>
        /// <example>2024-05-15</example>
        public string CreationDate { get; set; }

        /// <summary> Detailed list of branches the employee is associated with. </summary>
        public List<GetBranchDTO> Branches { get; set; }

        /// <summary> The name of the permission group assigned to the employee. </summary>
        /// <example>Accountant</example>
        public string Permission { get; set; }

        /// <summary> Flag indicating if the account is soft-deleted/deactivated. </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; }

        /// <summary> Employee's registered address. </summary>
        public string Address { get; set; }
    }
}
