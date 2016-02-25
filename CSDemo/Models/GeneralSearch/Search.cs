using System.Collections.Generic;
using CSDemo.Contracts;
using CSDemo.Contracts.GeneralSearch;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;

namespace CSDemo.Models.GeneralSearch
{
    public class Search : ISearch
    {
        public string Query { get; set; }
        public IList<Product.Product> Results { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public IEnumerable<CommerceQueryFacet> Facets { get; set; }
        public int CurrentPageNumber { get; set; }
    }
}