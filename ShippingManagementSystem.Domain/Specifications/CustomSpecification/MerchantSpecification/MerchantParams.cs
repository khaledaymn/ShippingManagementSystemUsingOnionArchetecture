using System;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification
{
    public class MerchantParams
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public MerchantParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
}