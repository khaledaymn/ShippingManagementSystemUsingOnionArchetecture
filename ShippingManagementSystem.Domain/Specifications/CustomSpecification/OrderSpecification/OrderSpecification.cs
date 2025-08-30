using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq.Expressions;

namespace ShippingManagementSystem.Domain.Specifications.OrderSpecification
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(OrderParams param)
        {
            // Define the filtering criteria
            Criteria = x =>
                (string.IsNullOrEmpty(param.Search) ||
                    x.CustomerName.ToLower().Contains(param.Search.ToLower()) ||
                    x.CustomerPhone1.Contains(param.Search) ||
                    (x.CustomerPhone2 != null && x.CustomerPhone2.Contains(param.Search))) &&
                (string.IsNullOrEmpty(param.OrderState) || x.OrderState.ToString() == param.OrderState) &&
                (!param.CityId.HasValue || x.CityId == param.CityId) &&
                (!param.BranchId.HasValue || x.BranchId == param.BranchId) &&
                (string.IsNullOrEmpty(param.MerchantId) || x.MerchantId == param.MerchantId) &&
                (string.IsNullOrEmpty(param.ShippingRepresentativeId) || x.ShippingRepresentativeId == param.ShippingRepresentativeId) &&
                (!param.FromDate.HasValue || x.CreationDate >= param.FromDate) &&
                (!param.ToDate.HasValue || x.CreationDate <= param.ToDate) &&
                (string.IsNullOrEmpty(param.OrderType) || x.OrderType.ToString() == param.OrderType) &&
                (string.IsNullOrEmpty(param.PaymentType) || x.PaymentType.ToString() == param.PaymentType) &&
                (!param.IsDeleted.HasValue || x.IsDeleted == param.IsDeleted);

            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "date":
                        ApplyOrderBy(x => x.CreationDate);
                        break;
                    case "date_desc":
                        ApplyOrderByDescending(x => x.CreationDate);
                        break;
                    case "customer":
                        ApplyOrderBy(x => x.CustomerName);
                        break;
                    case "customer_desc":
                        ApplyOrderByDescending(x => x.CustomerName);
                        break;
                    case "price":
                        ApplyOrderBy(x => x.OrderPrice);
                        break;
                    case "price_desc":
                        ApplyOrderByDescending(x => x.OrderPrice);
                        break;
                    case "charge":
                        ApplyOrderBy(x => x.ChargePrice);
                        break;
                    case "charge_desc":
                        ApplyOrderByDescending(x => x.ChargePrice);
                        break;
                    case "weight":
                        ApplyOrderBy(x => x.TotalWeight);
                        break;
                    case "weight_desc":
                        ApplyOrderByDescending(x => x.TotalWeight);
                        break;
                    case "amount":
                        ApplyOrderBy(x => x.AmountReceived);
                        break;
                    case "amount_desc":
                        ApplyOrderByDescending(x => x.AmountReceived);
                        break;
                    case "state":
                        ApplyOrderBy(x => x.OrderState);
                        break;
                    case "state_desc":
                        ApplyOrderByDescending(x => x.OrderState);
                        break;
                    default:
                        ApplyOrderByDescending(x => x.CreationDate);
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

        public OrderSpecification(Expression<Func<Order, bool>> criteria)
            : base(criteria)
        {
        }
        public OrderSpecification(int id)
            : base(x => x.Id == id)
        {
        }
        public OrderSpecification(string orderState) : base(x => x.OrderState.ToString() == orderState)
        {
            // Constructor for getting orders by status
            ApplyOrderByDescending(x => x.CreationDate);
        }
    }
}