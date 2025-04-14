using ShippingManagementSystem.Domain.Entities;
using ShippingManagementSystem.Domain.Enums;
using ShippingManagementSystem.Domain.Specifications;
using System;
using System.Linq;

namespace ShippingManagementSystem.Domain.Specifications.OrderSpecification
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(OrderParams param) : base(x =>
            // Search by customer name or phone
            (string.IsNullOrEmpty(param.Search) || 
                x.CustomerName.ToLower().Contains(param.Search.ToLower()) || 
                x.CustomerPhone1.Contains(param.Search) || 
                (x.CustomerPhone2 != null && x.CustomerPhone2.Contains(param.Search))) &&
            // Filter by order state
            (string.IsNullOrEmpty(param.OrderState) || 
                x.OrderState.ToString() == param.OrderState) &&
            // Filter by city
            (!param.CityId.HasValue || x.CityId == param.CityId) &&
            // Filter by branch
            (!param.BranchId.HasValue || x.BranchId == param.BranchId) &&
            // Filter by merchant
            (string.IsNullOrEmpty(param.MerchantId) || x.MerchantId == param.MerchantId) &&
            // Filter by shipping representative
            (string.IsNullOrEmpty(param.ShippigRepresentativeId) || x.ShippigRepresentativeId == param.ShippigRepresentativeId) &&
            // Filter by date range
            (!param.FromDate.HasValue || x.CreationDate >= param.FromDate) &&
            (!param.ToDate.HasValue || x.CreationDate <= param.ToDate) &&
            // Filter by order type
            (string.IsNullOrEmpty(param.OrderType) || x.OrderType.ToString() == param.OrderType) &&
            // Filter by payment type
            (string.IsNullOrEmpty(param.PaymentType) || x.PaymentType.ToString() == param.PaymentType) &&
            // Filter by deleted status
            (!param.IsDeleted.HasValue || x.IsDeleted == param.IsDeleted))
        {
            // Include related entities
            // We'll fetch these separately as needed
            
            // Apply sorting
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "date_asc":
                        ApplyOrderBy(x => x.CreationDate);
                        break;
                    case "date_desc":
                        ApplyOrderByDescending(x => x.CreationDate);
                        break;
                    case "customer_asc":
                        ApplyOrderBy(x => x.CustomerName);
                        break;
                    case "customer_desc":
                        ApplyOrderByDescending(x => x.CustomerName);
                        break;
                    case "price_asc":
                        ApplyOrderBy(x => x.OrderPrice);
                        break;
                    case "price_desc":
                        ApplyOrderByDescending(x => x.OrderPrice);
                        break;
                    default:
                        ApplyOrderByDescending(x => x.CreationDate);
                        break;
                }
            }
            else
            {
                // Default sorting by creation date (newest first)
                ApplyOrderByDescending(x => x.CreationDate);
            }

            // Apply pagination
            ApplyPagination(param.PageIndex, param.PageSize);
        }

        public OrderSpecification(int id) : base(x => x.Id == id)
        {
            // Constructor for getting order by ID
        }
        
        public OrderSpecification(string orderState) : base(x => x.OrderState.ToString() == orderState)
        {
            // Constructor for getting orders by status
            ApplyOrderByDescending(x => x.CreationDate);
        }
    }
} 