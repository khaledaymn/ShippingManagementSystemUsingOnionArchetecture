using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.GovernorateSpecification
{
    public class GovernorateSpecification : BaseSpecification<Governorate>
    {
        public GovernorateSpecification(GovernorateParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())) &&
            (!param.IsDeleted.HasValue || x.IsDeleted == param.IsDeleted))
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name_asc":
                        ApplyOrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(x => x.Name);
                        break;
                    default:
                        ApplyOrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(x => x.Id);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public GovernorateSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting governorate by ID
        }
    }
} 