using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;

namespace CSDemo.Contracts.GeneralSearch
{
    internal interface ISearchInfo
    {
        string SearchQuery { get; set; }
        IEnumerable<CommerceQueryFacet> RequiredFacets { get; set; }
        IEnumerable<CommerceQuerySort> SortFields { get; set; }
        int ItemsPerPage { get; set; }
        string CatalogName { get; set; }
        CommerceSearchOptions SearchOptions { get; set; }
    }
}
