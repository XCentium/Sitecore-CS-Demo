using System.Collections.Generic;
using CSDemo.Contracts;

namespace CSDemo.Models.GeneralSearch
{
    public class Search : ISearch
    {
        public string Query { get; set; }
        public IList<ISearchResult> Results { get; set; }
        public int PageSize { get; set; }
    }
}