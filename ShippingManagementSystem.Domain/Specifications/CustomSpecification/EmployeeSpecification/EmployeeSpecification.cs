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
            Criteria = m => !m.User.IsDeleted;
            ArgumentNullException.ThrowIfNull(param);

            Criteria = e =>
                (string.IsNullOrEmpty(param.Search) ||
                    e.User.Name.Contains(param.Search, StringComparison.OrdinalIgnoreCase) ||
                    e.User.Email.Contains(param.Search, StringComparison.OrdinalIgnoreCase) ||
                    e.User.PhoneNumber.Contains(param.Search, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(param.Branch) ||
                    e.User.UserBranches.Any(ub => ub.Branch.Name.Contains(param.Branch, StringComparison.OrdinalIgnoreCase))) &&
                (!param.IsActive.HasValue || e.User.IsDeleted == !param.IsActive);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "name":
                        ApplyOrderBy(e => e.User.Name);
                        break;
                    case "namedesc":
                        ApplyOrderByDescending(e => e.User.Name);
                        break;
                    case "email":
                        ApplyOrderBy(e => e.User.Email);
                        break;
                    case "emaildesc":
                        ApplyOrderByDescending(e => e.User.Email);
                        break;
                    case "phone":
                        ApplyOrderBy(e => e.User.PhoneNumber);
                        break;
                    case "phonedesc":
                        ApplyOrderByDescending(e => e.User.PhoneNumber);
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
