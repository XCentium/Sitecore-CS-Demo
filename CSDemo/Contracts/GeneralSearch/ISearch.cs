#region

using System.Collections.Generic;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;

#endregion

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

        string SortField { get; set; }

        #region Methods

        bool ContainsFacets(string url);
        string RemoveFacets(string url);
        string AddToFacets(string facetName, string value, string url);

        #endregion
    }
}