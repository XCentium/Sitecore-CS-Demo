using System.Collections.Generic;
using CSDemo.Models.GeneralSearch;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;

namespace CSDemo.Contracts.GeneralSearch
{
    public interface ISearch
    {
        string Query { get; set; }
        IList<Models.Product.Product> Results { get; set; }
        int PageSize { get; set; }

        int TotalItemCount { get; set; }
        int TotalPageCount { get; set; }
        IEnumerable<CommerceQueryFacet> Facets { get; set; }
        int CurrentPageNumber { get; set; }
        bool ContainsFacets(string url);
        string RemoveFacets(string url);
        string AddToFacets(string facetName, string value, string url);
    }
}