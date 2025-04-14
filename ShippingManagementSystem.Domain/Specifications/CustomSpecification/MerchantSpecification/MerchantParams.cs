using System;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification
{
    public class MerchantParams
    {
        private const int MaxPageSize = 10;
        private int _pageSize = 10;
        
        public int PageIndex { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}