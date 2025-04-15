using System;

namespace ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification
{
    public class ChargeTypeParams
    {
        public string? Search { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public ChargeTypeParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 