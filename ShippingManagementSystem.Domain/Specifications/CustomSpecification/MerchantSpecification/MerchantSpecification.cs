using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Domain.Specifications;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification
{
    public class MerchantSpecification : BaseSpecification<Merchant>
    {
        public MerchantSpecification(MerchantParams param)
        {
            // Apply filtering
            Criteria = m =>
                (string.IsNullOrEmpty(param.Search) ||
                    m.User.Name.ToLower().Contains(param.Search.ToLower()) ||
                    m.StoreName.ToLower().Contains(param.Search.ToLower()) ||
                    m.User.Email.ToLower().Contains(param.Search.ToLower()) ||
                    m.User.PhoneNumber.ToLower().Contains(param.Search.ToLower())) &&
                (!param.IsActive.HasValue || m.User.IsDeleted == !param.IsActive);

            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                        ApplyOrderBy(m => m.User.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(m => m.User.Name);
                        break;
                    case "storename":
                        ApplyOrderBy(m => m.StoreName);
                        break;
                    case "storename_desc":
                        ApplyOrderByDescending(m => m.StoreName);
                        break;
                    case "email":
                        ApplyOrderBy(m => m.User.Email);
                        break;
                    case "email_desc":
                        ApplyOrderByDescending(m => m.User.Email);
                        break;
                    case "rejectedpercentage":
                        ApplyOrderBy(m => m.RejectedOrederPercentage);
                        break;
                    case "rejectedpercentage_desc":
                        ApplyOrderByDescending(m => m.RejectedOrederPercentage);
                        break;
                    default:
                        ApplyOrderBy(m => m.User.Name);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(m => m.User.Name);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public MerchantSpecification(string id) : base(m => m.UserID == id)
        {
        }
    }
}