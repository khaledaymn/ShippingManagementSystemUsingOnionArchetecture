using System;

namespace ShippingManagementSystem.Domain.Specifications.GroupSpecification
{
    public class GroupParams
    {
        public string? Search { get; set; }
        public string? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GroupParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 