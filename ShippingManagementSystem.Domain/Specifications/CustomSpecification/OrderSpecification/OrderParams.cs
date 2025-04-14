using System;

namespace ShippingManagementSystem.Domain.Specifications.OrderSpecification
{
    public class OrderParams
    {
        public string? Search { get; set; }
        public string? OrderState { get; set; }
        public int? CityId { get; set; }
        public int? BranchId { get; set; }
        public string? MerchantId { get; set; }
        public string? ShippigRepresentativeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? OrderType { get; set; }
        public string? PaymentType { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public OrderParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 