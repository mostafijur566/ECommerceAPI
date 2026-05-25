using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Helper
{
    public class ProductQueryParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Search { get; set; }         // search by name
        public Guid? CategoryId { get; set; }        // filter by category
        public decimal? MinPrice { get; set; }       // filter by min price
        public decimal? MaxPrice { get; set; }       // filter by max price
        public string? SortBy { get; set; }          // name, price, createdat
        public string? SortOrder { get; set; } = "asc"; // asc, desc
    }
}