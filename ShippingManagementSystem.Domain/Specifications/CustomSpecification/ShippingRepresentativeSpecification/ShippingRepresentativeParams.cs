using System;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification
{
    public class ShippingRepresentativeParams
    {
        private const int MaxPageSize = 10;
        private int _pageSize = 100;
        
        public int PageIndex { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search { get; set; }
        public string? Branch { get; set; }
        public bool? IsActive { get; set; }
        public string? Sort { get; set; }
        public int? GovernorateId { get; set; }
    }
}