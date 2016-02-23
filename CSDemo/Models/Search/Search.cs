using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts;

namespace CSDemo.Models.Search
{
    public class Search : ISearch
    {
        public string Query { get; set; }
        public IList<ISearchResult> Results { get; set; }
        public int PageSize { get; set; }
    }
}