using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingManagementSystem.Domain.DTOs
{
    public class PaginationResponse<T>
    {
        /// <summary> Number of items requested per page. </summary>
        /// <example>10</example>
        public int PageSize { get; set; }

        /// <summary> Current page index (1-based). </summary>
        /// <example>1</example>
        public int PageIndex { get; set; }

        /// <summary> Total count of records available across all pages. </summary>
        /// <example>150</example>
        public int TotalCount { get; set; }

        /// <summary> The collection of data for the current page. </summary>
        public IReadOnlyList<T> Data { get; set; }

        public PaginationResponse(int pageSize, int pageIndex, int totalCount, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
