﻿using ShippingManagementSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; init; }
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginationEnabled { get; private set; }

        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            if (pageIndex < 1 || pageSize < 1)
                throw new ArgumentException("PageIndex and PageSize must be positive.");
            IsPaginationEnabled = true;
            Skip = pageSize * (pageIndex - 1);
            Take = pageSize;
        }
    }
}
