using Microsoft.AspNetCore.Identity;
using ShippingManagementSystem.Domain.Specifications;
using ShippingManagementSystem.Domain.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications.CustomSpecification.EmployeeSpecification
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {

        public EmployeeSpecification(EmployeeParams param)
        {

            Criteria = e =>
                (string.IsNullOrEmpty(param.Search) ||
                    e.User.Name.ToLower().Contains(param.Search.ToLower()) ||
                    e.User.Email.ToLower().Contains(param.Search.ToLower())) &&
                (!param.IsActive.HasValue || e.User.IsDeleted == !param.IsActive);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                        ApplyOrderBy(e => e.User.Name);
                        break;
                    case "name_desc":
                        ApplyOrderByDescending(e => e.User.Name);
                        break;
                    case "email":
                        ApplyOrderBy(e => e.User.Email);
                        break;
                    case "email_desc":
                        ApplyOrderByDescending(e => e.User.Email);
                        break;
                    default:
                        ApplyOrderBy(e => e.User.Name);
                        break;
                }
            }

            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public EmployeeSpecification(Expression<Func<Employee, bool>> criteria)
            : base(criteria)
        {
        }
    }
}
