using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.ShippingRepresentativeSpecification
{
    public class ShippingRepresentativeSpecification : BaseSpecification<ShippigRepresentative>
    {
        public ShippingRepresentativeSpecification(ShippingRepresentativeParams param) : base()
        {
            // Apply filtering
            Expression<Func<ShippigRepresentative, bool>> searchCriteria = null;

            if (!string.IsNullOrEmpty(param.Search))
            {
                searchCriteria = sr => 
                    sr.User.Name.Contains(param.Search) ||
                    sr.User.Email.Contains(param.Search) ||
                    sr.User.UserName.Contains(param.Search) ||
                    sr.User.IsDeleted == param.IsActive ||
                    sr.User.PhoneNumber.Contains(param.Search);
            }
            
            // Filter by governorate if specified
            if (param.GovernorateId.HasValue)
            {
                var governorateCriteria = (Expression<Func<ShippigRepresentative, bool>>)(sr => 
                    sr.ShippingRepGovernorates.Any(g => g.GovernorateId == param.GovernorateId.Value));
                
                if (searchCriteria != null)
                {
                    // Combine criteria
                    var parameter = Expression.Parameter(typeof(ShippigRepresentative), "sr");
                    var body = Expression.AndAlso(
                        Expression.Invoke(searchCriteria, parameter),
                        Expression.Invoke(governorateCriteria, parameter)
                    );
                    searchCriteria = Expression.Lambda<Func<ShippigRepresentative, bool>>(body, parameter);
                }
                else
                {
                    searchCriteria = governorateCriteria;
                }
            }
            
            // Set the criteria
            if (searchCriteria != null)
            {
                Criteria = searchCriteria;
            }
            
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                            ApplyOrderBy(sr => sr.User.Name);
                        break;
                    case "email":
                            ApplyOrderBy(sr => sr.User.Email);
                        break;
                    case "companypercentage":
                            ApplyOrderBy(sr => sr.CompanyPersentage);
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
            
            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }
        
        public ShippingRepresentativeSpecification(string id) : base(sr => sr.UserID == id)
        {
            // No includes needed with lazy loading
        }
    }
}