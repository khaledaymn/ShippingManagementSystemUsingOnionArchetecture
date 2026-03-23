using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs.GroupDTOs
{
    public class GroupPermissionsDTO
    {
        /// <summary> Group ID. </summary>
        public int Id { get; set; }

        /// <summary> Group Name. </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> Creation Date (Formatted as dd/MM/yyyy). </summary>
        /// <example>15/05/2024</example>
        public string Date { get; set; }

        /// <summary> 
        /// Matrix of permissions where the Key is the Module Name 
        /// and the Value is a list of Permission IDs (Enum values).
        /// </summary>
        /// <example> { "Orders": [1, 2, 3], "Users": [1] } </example>
        public Dictionary<string, List<int>> Permissions { get; set; }
    }
}
