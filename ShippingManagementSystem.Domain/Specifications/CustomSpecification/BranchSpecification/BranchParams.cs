using System;

namespace ShippingManagementSystem.Domain.Specifications.BranchSpecification
{
    public class BranchParams
    {
        /// <summary> Search term to filter branches by name. </summary>
        /// <example>Cairo</example>
        public string? Search { get; set; }

        /// <summary> Filter branches by a specific location or address keyword. </summary>
        /// <example>Industrial</example>
        public string? Location { get; set; }

        /// <summary> Filter branches belonging to a specific City ID. </summary>
        /// <example>10</example>
        public int? CityId { get; set; }

        /// <summary> Filter by activation status (Soft-deleted or active). </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }

        /// <summary> Sorting criteria (e.g., 'name_asc', 'name_desc'). </summary>
        /// <example>name_asc</example>
        public string? Sort { get; set; }

        /// <summary> Current page index for pagination. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of items per page (Maximum 10). </summary>
        /// <example>5</example>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Initializes a new instance of BranchParams with a clamped PageSize.
        /// </summary>
        public BranchParams()
        {
            // Ensures PageSize stays within the logical system bounds (1-10)
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 