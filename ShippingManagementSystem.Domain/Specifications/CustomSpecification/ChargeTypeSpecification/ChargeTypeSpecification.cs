using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.ChargeTypeSpecification
{
    public class ChargeTypeSpecification : BaseSpecification<ChargeType>
    {
        public ChargeTypeSpecification(ChargeTypeParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())) &&
            (!param.MinPrice.HasValue || x.ExtraPrice >= param.MinPrice) &&
            (!param.MaxPrice.HasValue || x.ExtraPrice <= param.MaxPrice) &&
            (!param.MinDays.HasValue || x.NumOfDay >= param.MinDays) &&
            (!param.MaxDays.HasValue || x.NumOfDay <= param.MaxDays) &&
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
                    case "price_asc":
                        ApplyOrderBy(x => x.ExtraPrice);
                        break;
                    case "price_desc":
                        ApplyOrderByDescending(x => x.ExtraPrice);
                        break;
                    case "days_asc":
                        ApplyOrderBy(x => x.NumOfDay);
                        break;
                    case "days_desc":
                        ApplyOrderByDescending(x => x.NumOfDay);
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

        public ChargeTypeSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting charge type by ID
        }
    }
} 