using System;

namespace ShippingManagementSystem.Domain.Specifications.BranchSpecification
{
    public class BranchParams
    {
        public string? Search { get; set; }
        public string? Location { get; set; }
        public int? CityId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public BranchParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }
    }
} 