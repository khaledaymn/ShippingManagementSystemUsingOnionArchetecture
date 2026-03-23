using System;

namespace ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification
{
    public class ChargeTypeParams
    {
        /// <summary> Search by name keyword. </summary>
        /// <example>Express</example>
        public string? Search { get; set; }

        /// <summary> Minimum extra price for filtering. </summary>
        /// <example>10.0</example>
        public double? MinPrice { get; set; }

        /// <summary> Maximum extra price for filtering. </summary>
        /// <example>50.0</example>
        public double? MaxPrice { get; set; }

        /// <summary> Minimum delivery days for filtering. </summary>
        /// <example>1</example>
        public int? MinDays { get; set; }

        /// <summary> Maximum delivery days for filtering. </summary>
        /// <example>7</example>
        public int? MaxDays { get; set; }

        /// <summary> Filter by activation status. </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }

        /// <summary> Sorting expression (e.g., 'price_desc', 'days_asc'). </summary>
        /// <example>price_asc</example>
        public string? Sort { get; set; }

        /// <summary> Current page index. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Number of records per page (Clamped between 1 and 100). </summary>
        /// <example>20</example>
        public int PageSize { get; set; } = 100;

        /// <summary>
        /// Initializes ChargeTypeParams with strict page size clamping for performance.
        /// </summary>
        public ChargeTypeParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 