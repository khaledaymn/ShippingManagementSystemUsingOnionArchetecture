using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.MeduleDTOs
{
    public class Permission
    {
        /// <summary> The ID of the Module (e.g., Orders = 1, Branches = 2). </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary> List of permission enum values (e.g., [1, 2] representing View and Create). </summary>
        /// <example>[1, 2, 3, 4]</example>
        public List<int> Values { get; set; }
    }
}
