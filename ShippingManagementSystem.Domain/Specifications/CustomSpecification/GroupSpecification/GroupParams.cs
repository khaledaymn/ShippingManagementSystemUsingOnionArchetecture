using System;

namespace ShippingManagementSystem.Domain.Specifications.GroupSpecification
{
    public class GroupParams
    {
        /// <summary> Search by group name keyword. </summary>
        /// <example>Admin</example>
        public string? Search { get; set; }

        /// <summary> Filter groups created by or associated with a specific user ID. </summary>
        public string? UserId { get; set; }

        /// <summary> Start date for filtering groups created within a specific range. </summary>
        public DateTime? FromDate { get; set; }

        /// <summary> End date for filtering groups created within a specific range. </summary>
        public DateTime? ToDate { get; set; }

        /// <summary> Sorting expression (e.g., 'name_asc', 'date_desc'). </summary>
        /// <example>date_desc</example>
        public string? Sort { get; set; }

        /// <summary> Current page index. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of records per page (Clamped between 1 and 100). </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 100;

        public GroupParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 