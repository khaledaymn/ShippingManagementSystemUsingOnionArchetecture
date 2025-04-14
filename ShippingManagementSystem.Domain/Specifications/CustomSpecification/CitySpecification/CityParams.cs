using System;

namespace ShippingManagementSystem.Domain.Specifications.CitySpecification
{
    public class CityParams
    {
        public string? Search { get; set; }
        public int? GovernorateId { get; set; }
        public double? MinChargePrice { get; set; }
        public double? MaxChargePrice { get; set; }
        public double? MinPickUpPrice { get; set; }
        public double? MaxPickUpPrice { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public CityParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 