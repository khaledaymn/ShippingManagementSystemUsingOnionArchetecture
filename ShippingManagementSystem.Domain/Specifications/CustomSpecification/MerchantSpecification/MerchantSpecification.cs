using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.MerchantSpecification
{
    public class MerchantSpecification : BaseSpecification<Merchant>
    {
        public MerchantSpecification(MerchantParams param) : base()
        {
            Criteria = m => !m.User.IsDeleted;
            // Apply filtering
            if (!string.IsNullOrEmpty(param.Search))
            {
                Criteria =(m =>
                    m.User.Name.Contains(param.Search) ||
                    m.StoreName.Contains(param.Search) ||
                    m.User.Email.Contains(param.Search) ||
                    m.User.PhoneNumber.Contains(param.Search));
            }
            
            // Apply sorting
            if (!string.IsNullOrEmpty(param.SortBy))
            {
                switch (param.SortBy.ToLower())
                {
                    case "name":
                        if (param.SortDirection.ToLower() == "desc")
                            ApplyOrderByDescending(m => m.User.Name);
                        else
                            ApplyOrderBy(m => m.User.Name);
                        break;
                    case "storename":
                        if (param.SortDirection.ToLower() == "desc")
                            ApplyOrderByDescending(m => m.StoreName);
                        else
                            ApplyOrderBy(m => m.StoreName);
                        break;
                    case "email":
                        if (param.SortDirection.ToLower() == "desc")
                            ApplyOrderByDescending(m => m.User.Email);
                        else
                            ApplyOrderBy(m => m.User.Email);
                        break;
                    case "rejectedpercentage":
                        if (param.SortDirection.ToLower() == "desc")
                            ApplyOrderByDescending(m => m.RejectedOrederPercentage);
                        else
                            ApplyOrderBy(m => m.RejectedOrederPercentage);
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
            // No includes needed with lazy loading
        }
    }
}