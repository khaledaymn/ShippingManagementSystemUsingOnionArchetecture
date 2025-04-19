using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerhantSpeialPricesSpeifications
{
    public class MerchantSpecialPricesParams
    {
        public string? Search { get; set; }
        
        public int? CityId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public MerchantSpecialPricesParams()
        {
            PageSize = Math.Clamp(PageSize, 1, 100);
        }

    }
}
