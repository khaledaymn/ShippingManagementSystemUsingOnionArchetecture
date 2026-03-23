using System;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification
{
    public class ShippingRepresentativeParams
    {
        /// <summary> Current results page. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Items per page (Maximum 10). </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;

        /// <summary> Search by Name, Email, or Phone. </summary>
        /// <example>Mahmoud</example>
        public string? Search { get; set; }

        /// <summary> Filter by associated Branch name. </summary>
        /// <example>Cairo Main</example>
        public string? Branch { get; set; }

        /// <summary> Filter by account activity status. </summary>
        /// <example>true</example>
        public bool? IsActive { get; set; }

        /// <summary> Sort expression (e.g., 'name_asc', 'hiring_desc'). </summary>
        /// <example>hiring_desc</example>
        public string? Sort { get; set; }

        /// <summary> Filter by specific Governorate ID. </summary>
        /// <example>3</example>
        public int? GovernorateId { get; set; }
    }
}