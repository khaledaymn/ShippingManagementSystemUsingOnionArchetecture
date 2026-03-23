using System;

namespace ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification
{
    public class RejectedReasonParams
    {
        /// <summary> Search term to filter reasons by their descriptive text. </summary>
        /// <example>Customer</example>
        public string? Search { get; set; }

        /// <summary> Filter by activation status. </summary>
        public bool? IsDeleted { get; set; }

        /// <summary> Sorting expression (e.g., 'text_asc', 'text_desc'). </summary>
        public string? Sort { get; set; }

        /// <summary> Current page index. </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of records per page (Clamped between 1 and 100). </summary>
        public int PageSize { get; set; } = 100;

        public RejectedReasonParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 