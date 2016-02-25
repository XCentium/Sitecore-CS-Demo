using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSDemo.Contracts;
using CSDemo.Contracts.GeneralSearch;
using CSDemo.Models.GeneralSearch;
using CSDemo.Models.Product;
using CSDemo.Services;
using Glass.Mapper.Sc;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Search;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Services;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;

namespace CSDemo.Controllers
{
    public class GeneralSearchController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreService _service;

        #endregion

        #region Constructors

        public GeneralSearchController(ISitecoreService service)
        {
            _service = service;
        }

        public GeneralSearchController() : this(new SitecoreContext()) { }

        #endregion

        #region Methods


        public ActionResult SearchResults(
            [Bind(Prefix = Constants.QueryStrings.SearchQuery)] string query,
            
            [Bind(Prefix = Constants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = Constants.QueryStrings.Sort)] string sortField = "Title",
            [Bind(Prefix = Constants.QueryStrings.PageSize)] int pageSize=50,
            [Bind(Prefix = Constants.QueryStrings.SortDirection)] CommerceConstants.SortDirection sortDirection = CommerceConstants.SortDirection.Desc,
            [Bind(Prefix = Constants.QueryStrings.Paging)] int pageNumber = 1)
        {
            var searchModel = new Search();
            if (string.IsNullOrWhiteSpace(query)) return View(searchModel);

            var searchInfo = GetSearchInfo(query, pageNumber, facetValues, sortField, pageSize, sortDirection);

            searchModel = GetSearchModel(searchInfo.SearchOptions, searchInfo.SearchQuery, searchInfo.CatalogName);


            return View(searchModel);
        }

        [HttpGet]
        public ActionResult SearchInput()
        {
            var searchResultsPagePath = RenderingContext.Current.Rendering.DataSource;
            if(string.IsNullOrWhiteSpace(searchResultsPagePath))return View(new SearchInput());

            var searchResultsPageItem = _service.Database.GetItem(searchResultsPagePath);
            if (searchResultsPageItem == null) return View(new SearchInput());

            var redirectUrl = LinkManager.GetItemUrl(searchResultsPageItem);
            var searchInputModel = new SearchInput
            {
                RedirectUrl = redirectUrl
            };
            return View(searchInputModel);
        }

        [HttpPost]
        public ActionResult SearchInput(SearchInput model)
        {
            if (!ModelState.IsValid 
                || string.IsNullOrWhiteSpace(model.RedirectUrl) 
                || string.IsNullOrWhiteSpace(model.Query))
                return SearchInput();
            
            return Redirect($"{model.RedirectUrl}?{Constants.QueryStrings.SearchQuery}={HttpUtility.UrlEncode(model.Query)}");
        }

        #region Private Helpers

        private ISearchInfo GetSearchInfo(string searchKeyword, int pageNumber, string facetValues, string sortField, int pageSize, CommerceConstants.SortDirection? sortDirection)
        {
            //var searchManager = CommerceTypeLoader.CreateInstance<ICommerceSearchManager>();
            var searchInfo = new SearchInfo
            {
                SearchQuery = searchKeyword ?? string.Empty,
                RequiredFacets = null, //searchManager.GetFacetFieldsForItem(this.Item),
                SortFields = null, // searchManager.GetSortFieldsForItem(this.Item),
                CatalogName =  Constants.Commerce.CatalogName,
                ItemsPerPage = pageSize 
            };

            var productSearchOptions = new CommerceSearchOptions(searchInfo.ItemsPerPage, pageNumber-1);
            searchInfo.SearchOptions = productSearchOptions;

            return searchInfo;
        }


        private Search GetSearchModel(CommerceSearchOptions searchOptions, string searchKeyword, string catalogName)
        {
            using (new SecurityDisabler())
            {
                Assert.ArgumentNotNull(searchKeyword, "searchOptions");
                Assert.ArgumentNotNull(searchKeyword, "searchKeyword");
                Assert.ArgumentNotNull(searchKeyword, "catalogName");

                var returnList = new List<Item>();
                var totalPageCount = 0;
                var totalProductCount = 0;
                var facets = Enumerable.Empty<CommerceQueryFacet>();

                if (!string.IsNullOrEmpty(searchKeyword.Trim()))
                {
                    SearchResponse searchResponse = null;
                    searchResponse = SearchCatalogItemsByKeyword(searchKeyword, catalogName, searchOptions);

                    if (searchResponse != null)
                    {
                        returnList.AddRange(searchResponse.ResponseItems);
                        totalProductCount = searchResponse.TotalItemCount;
                        totalPageCount = searchResponse.TotalPageCount;
                        facets = searchResponse.Facets;
                    }
                }
                Sitecore.Context.Item.Fields.ReadAll();
                var result = new Search
                {
                    TotalItemCount = totalProductCount,
                    TotalPageCount = totalPageCount,
                    CurrentPageNumber = searchOptions.StartPageIndex,
                    Facets = facets,
                    Results = returnList.Select(i=> i.GlassCast<Product>()).ToList(),
                    Query = searchKeyword
                };

                return result;
            }
        }

        private static SearchResponse SearchCatalogItemsByKeyword(string keyword, string catalogName, CommerceSearchOptions searchOptions)
        {
            Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");
            var searchManager = CommerceTypeLoader.CreateInstance<ICommerceSearchManager>();
            var searchIndex = searchManager.GetIndex(catalogName);

            using (var context = searchIndex.CreateSearchContext())
            {
                var searchResults = context.GetQueryable<CommerceProductSearchResultItem>()
                    .Where(item => item.Content.Contains(keyword))
                    .Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
                    .Where(item => item.CatalogName == catalogName)
                    .Where(item => item.Language == Sitecore.Context.Language.Name)
                    .Select(p => new CommerceProductSearchResultItem()
                    {
                        ItemId = p.ItemId,
                        Uri = p.Uri
                    });

                searchResults = searchManager.AddSearchOptionsToQuery<CommerceProductSearchResultItem>(searchResults, searchOptions);

                var results = searchResults.GetResults();
                var response = SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

                return response;
            }
        }

        #endregion

        #endregion
    }
}