using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.RejectedReasonSpecification
{
    public class RejectedReasonSpecification : BaseSpecification<RejectedReason>
    {
        public RejectedReasonSpecification(RejectedReasonParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Text.ToLower().Contains(param.Search.ToLower())) &&
            (!param.IsDeleted.HasValue || x.IsDeleted == param.IsDeleted))
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "text_asc":
                        ApplyOrderBy(x => x.Text);
                        break;
                    case "text_desc":
                        ApplyOrderByDescending(x => x.Text);
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

        public RejectedReasonSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting rejected reason by ID
        }
    }
} 