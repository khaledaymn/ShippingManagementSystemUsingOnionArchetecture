using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerhantSpeialPricesSpeifications
{
  public class MerchantSpecialSpecifications:BaseSpecification<MerchantSpecialPrice>
    {
        public MerchantSpecialSpecifications(MerchantSpecialPricesParams param) : base(x =>
           (string.IsNullOrEmpty(param.Search) || x.City.Name.ToLower().Contains(param.Search.ToLower())) 
           )
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name_asc":
                        ApplyOrderBy(x => x.City.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(x => x.City.Name);
                        break;
                    
                    default:
                        ApplyOrderBy(x => x.MerchantId);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(x => x.MerchantId);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public MerchantSpecialSpecifications(string id) : base(x =>x.MerchantId==id)
        {
            // Constructor for getting branch by ID
        }
    }
}
