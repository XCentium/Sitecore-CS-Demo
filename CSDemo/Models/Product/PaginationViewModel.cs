using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class PaginationViewModel
    {
        public string Category { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string OrderBy { get; set; }
        public string CategoryID { get; set; }
        public int TotalItems { get; set; }
        public long TotalPages { get; set; }

    }
}