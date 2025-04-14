using System;

namespace ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification
{
    public class RejectedReasonParams
    {
        public string? Search { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public RejectedReasonParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 