using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementAPI.Helpers
{
    public class QueryObject
    {
        public string? SearchQuery { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public string? SortOrder { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}