using System;

namespace ShippingManagementSystem.Domain.Specifications.CitySpecification
{
    public class CityParams
    {
        /// <summary> Search by city name. </summary>
        /// <example>Maadi</example>
        public string? Search { get; set; }

        /// <summary> Filter cities belonging to a specific Governorate ID. </summary>
        /// <example>1</example>
        public int? GovernorateId { get; set; }

        /// <summary> Minimum shipping charge for filtering price ranges. </summary>
        /// <example>30.0</example>
        public double? MinChargePrice { get; set; }

        /// <summary> Maximum shipping charge for filtering price ranges. </summary>
        /// <example>100.0</example>
        public double? MaxChargePrice { get; set; }

        /// <summary> Minimum pickup fee for filtering. </summary>
        /// <example>5.0</example>
        public double? MinPickUpPrice { get; set; }

        /// <summary> Maximum pickup fee for filtering. </summary>
        /// <example>25.0</example>
        public double? MaxPickUpPrice { get; set; }

        /// <summary> Filter by activation status. </summary>
        /// <example>false</example>
        public bool? IsDeleted { get; set; }

        /// <summary> Sort expression (e.g., 'price_asc', 'name_desc'). </summary>
        /// <example>price_asc</example>
        public string? Sort { get; set; }

        /// <summary> Current page index. </summary>
        /// <example>1</example>
        public int PageIndex { get; set; } = 1;

        /// <summary> Items per page (Clamped between 1 and 20). </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Initializes a new instance of CityParams with strict page size clamping.
        /// </summary>
        public CityParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 20);
        }
    }
} 