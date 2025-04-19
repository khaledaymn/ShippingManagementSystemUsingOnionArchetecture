using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications.BranchSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.UserBranches
{
   public class UserBranchSpeification: BaseSpecification<ShippingManagementSystem.Domain.Entities.UserBranches>
    {

        public UserBranchSpeification(UserBranchesParams param) : base(x =>
          (string.IsNullOrEmpty(param.Search) || x.Branch.Name.ToLower().Contains(param.Search.ToLower())) 
        
         
        )
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name_asc":
                        ApplyOrderBy(x => x.Branch.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(x => x.Branch.Name);
                        break;
                    
                    default:
                        ApplyOrderBy(x => x.UserId);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(x => x.UserId);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }
        public UserBranchSpeification(string id) : base(x => x.UserId == id)
        {
            
        }
    }
}
