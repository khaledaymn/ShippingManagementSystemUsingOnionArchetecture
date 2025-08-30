using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification
{
    public class ShippingRepresentativeSpecification : BaseSpecification<ShippigRepresentative>
    {
        public ShippingRepresentativeSpecification(ShippingRepresentativeParams param)
        {
            Criteria = sr =>
                (string.IsNullOrEmpty(param.Search) ||
                    sr.User.Name.ToLower().Contains(param.Search.ToLower()) ||
                    sr.User.Email.ToLower().Contains(param.Search.ToLower()) ||
                    sr.User.UserName.ToLower().Contains(param.Search.ToLower()) ||
                    sr.User.PhoneNumber.ToLower().Contains(param.Search.ToLower())) &&
                (!param.IsActive.HasValue || sr.User.IsDeleted == !param.IsActive) &&
                (!param.GovernorateId.HasValue || sr.ShippingRepGovernorates.Any(g => g.GovernorateId == param.GovernorateId.Value));

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                        ApplyOrderBy(sr => sr.User.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(sr => sr.User.Name);
                        break;
                    case "email":
                        ApplyOrderBy(sr => sr.User.Email);
                        break;
                    case "email_desc":
                        ApplyOrderByDescending(sr => sr.User.Email);
                        break;
                    case "companypercentage":
                        ApplyOrderBy(sr => sr.CompanyPersentage);
                        break;
                    case "companypercentage_desc":
                        ApplyOrderByDescending(sr => sr.CompanyPersentage);
                        break;
                    default:
                        ApplyOrderBy(sr => sr.User.Name);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(sr => sr.User.Name);
            }

            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public ShippingRepresentativeSpecification(string id) : base(sr => sr.UserID == id)
        {
        }
    }
}