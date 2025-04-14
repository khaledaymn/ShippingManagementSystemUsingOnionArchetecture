using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.GroupSpecification
{
    public class GroupSpecification : BaseSpecification<Group>
    {
        public GroupSpecification(GroupParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())) &&
            (string.IsNullOrEmpty(param.UserId) || x.User.Any( e => e.UserID == param.UserId)) &&
            (!param.FromDate.HasValue || x.CreationDate >= param.FromDate) &&
            (!param.ToDate.HasValue || x.CreationDate <= param.ToDate))
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
                ApplyOrderByDescending(x => x.CreationDate);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public GroupSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting group by ID
        }
    }
} 