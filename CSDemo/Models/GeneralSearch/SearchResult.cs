using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts;
using CSDemo.Contracts.GeneralSearch;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Models.Search
{
    public class SearchResult : ISearchResult
    {
        public Image Image { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsNew { get; set; }
    }
}