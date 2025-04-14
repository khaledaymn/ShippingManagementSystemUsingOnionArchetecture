using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.CitySpecification
{
    public class CitySpecification : BaseSpecification<City>
    {
        public CitySpecification(CityParams param) : base(x =>
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search.ToLower())) &&
            (!param.GovernorateId.HasValue || x.GovernorateId == param.GovernorateId) &&
            (!param.MinChargePrice.HasValue || x.ChargePrice >= param.MinChargePrice) &&
            (!param.MaxChargePrice.HasValue || x.ChargePrice <= param.MaxChargePrice) &&
            (!param.MinPickUpPrice.HasValue || x.PickUpPrice >= param.MinPickUpPrice) &&
            (!param.MaxPickUpPrice.HasValue || x.PickUpPrice <= param.MaxPickUpPrice) &&
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
                    case "charge_asc":
                        ApplyOrderBy(x => x.ChargePrice);
                        break;
                    case "charge_desc":
                        ApplyOrderByDescending(x => x.ChargePrice);
                        break;
                    case "pickup_asc":
                        ApplyOrderBy(x => x.PickUpPrice);
                        break;
                    case "pickup_desc":
                        ApplyOrderByDescending(x => x.PickUpPrice);
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

        public CitySpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting city by ID
        }
    }
} 