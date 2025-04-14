using System;

namespace ShippingManagementSystem.Domain.Specifications.GovernorateSpecification
{
    public class GovernorateParams
    {
        public string? Search { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GovernorateParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 