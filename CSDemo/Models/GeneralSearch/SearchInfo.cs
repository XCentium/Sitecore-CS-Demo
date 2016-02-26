#region

using System.Collections.Generic;
using CSDemo.Contracts.GeneralSearch;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;

#endregion

namespace CSDemo.Models.GeneralSearch
{
    internal class SearchInfo : ISearchInfo
    {
        public string SearchQuery { get; set; }

        public IEnumerable<CommerceQueryFacet> RequiredFacets { get; set; }

        public IEnumerable<CommerceQuerySort> SortFields { get; set; }

        public int ItemsPerPage { get; set; }

        public string CatalogName { get; set; }

        public CommerceSearchOptions SearchOptions { get; set; }
    }
}