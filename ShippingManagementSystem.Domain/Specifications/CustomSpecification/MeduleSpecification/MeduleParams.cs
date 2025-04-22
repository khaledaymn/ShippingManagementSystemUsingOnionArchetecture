using System;

namespace ShippingManagementSystem.Domain.Specifications.MeduleSpecification
{
    public class MeduleParams
    {
        public string? Search { get; set; }
        public string? UserId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public MeduleParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 10);
        }
    }
} 