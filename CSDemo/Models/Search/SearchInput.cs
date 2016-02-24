using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts;

namespace CSDemo.Models.Search
{
    public class SearchInput : ISearchInput
    {
        public string Query { get; set; }
    }
}