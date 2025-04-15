using System;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification
{
    public class ShippingRepresentativeParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 100;
        
        public int PageIndex { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public int? GovernorateId { get; set; }
    }
}