using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification
{
    public class EmployeeParams
    {
        /// <summary> Search by Name, Email, or Phone Number. </summary>
        /// <example>Ahmed</example>
        public string? Search { get; set; }

        /// <summary> Filter by activation status (Active/Deactivated). </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }

        /// <summary> Sort by specific fields (e.g., name, creationDate). </summary>
        /// <example>name_desc</example>
        public string? Sort { get; set; }

        /// <summary> Current page number. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of records per page (Clamped between 1 and 10). </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;

        public EmployeeParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
}
