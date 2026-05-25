using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Helper
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public static PagedResponse<T> Create(List<T> data, int page, int pageSize, int totalRecords)
        {
            return new PagedResponse<T>
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                HasNextPage = page < (int)Math.Ceiling(totalRecords / (double)pageSize),
                HasPreviousPage = page > 1
            };
        }
    }
}