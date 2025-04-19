using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.BranchSpecification
{
    public class BranchSpecification : BaseSpecification<Branch>
    {
        public BranchSpecification(BranchParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())) &&
            (string.IsNullOrEmpty(param.Location) || x.Location.ToLower().Contains(param.Location.ToLower())) &&
            (!param.CityId.HasValue || x.CityId == param.CityId) &&
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
                    case "date_asc":
                        ApplyOrderBy(x => x.CreationDate);
                        break;
                    case "date_desc":
                        ApplyOrderByDescending(x => x.CreationDate);
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

        public BranchSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting branch by ID
        }
        
    }
} 