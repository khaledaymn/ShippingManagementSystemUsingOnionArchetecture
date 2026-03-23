using System;

namespace ShippingManagementSystem.Domain.Specifications.GovernorateSpecification
{
    public class GovernorateParams
    {
        /// <summary> Search term to filter governorates by name. </summary>
        /// <example>Cai</example>
        public string? Search { get; set; }

        /// <summary> Filter results by deletion status. </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }

        /// <summary> Sorting criteria (e.g., 'name_asc', 'name_desc'). </summary>
        /// <example>name_asc</example>
        public string? Sort { get; set; }

        /// <summary> The current page index for pagination. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of items per page (Maximum 100). </summary>
        /// <example>50</example>
        public int PageSize { get; set; } = 100;

        /// <summary>
        /// Initializes a new instance of GovernorateParams with a clamped PageSize.
        /// </summary>
        public GovernorateParams()
        {
            // Ensures PageSize stays within the logical system bounds (1-100)
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 