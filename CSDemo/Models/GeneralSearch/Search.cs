using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public string AddToFacets(string facetName, string value, string url)
        {
            if (string.IsNullOrWhiteSpace(facetName) || string.IsNullOrWhiteSpace(value)
                || string.IsNullOrWhiteSpace(url)) return string.Empty;

            url = HttpUtility.UrlDecode(url);

            var uri = new Uri(url);
            var queryStrings = HttpUtility.ParseQueryString(uri.Query);
            var facetQueryString = queryStrings.Get(Constants.QueryStrings.Facets);

            if (string.IsNullOrEmpty(facetQueryString))
            {
                queryStrings.Add(Constants.QueryStrings.Facets, 
                    HttpUtility.UrlEncode($"{facetName}{Constants.QueryStrings.FacetOptionDefinitionSeparator}{value}"));
            }
            else
            {
                if (!facetQueryString.Contains(Constants.QueryStrings.FacetsSeparator) ||
                    !facetQueryString.Contains("=")) return string.Empty;

                queryStrings.Remove(Constants.QueryStrings.Facets);

                var facetsPairs = facetQueryString.Split(new string[] { Constants.QueryStrings.FacetOptionSeparator }, StringSplitOptions.None);
                var updatedFacetPairs = new List<string>();
                var isNewFacetPair = true;
                foreach (var facetsPair in facetsPairs)
                {
                    var facetParts = facetsPair.Split(Constants.QueryStrings.FacetOptionDefinitionSeparator);
                    var facetPairKey = facetParts[0];
                    

                    if (facetPairKey.ToLower() != facetName.ToLower())
                    {
                        updatedFacetPairs.Add(facetsPair);
                        continue;
                    }
                        
                    isNewFacetPair = false;
                    var facetPairValue = facetParts[1];
                    var newFacetPair = $"{facetPairKey}{Constants.QueryStrings.FacetOptionDefinitionSeparator}{facetPairValue}{Constants.QueryStrings.FacetsSeparator}{value}";
                    updatedFacetPairs.Add(newFacetPair);
                }

                var newFacetsValue = string.Join(Constants.QueryStrings.FacetOptionSeparator, updatedFacetPairs);
                newFacetsValue = isNewFacetPair ? $"{newFacetsValue}{Constants.QueryStrings.FacetOptionSeparator}{facetName}{Constants.QueryStrings.FacetOptionDefinitionSeparator}{value}" : newFacetsValue;
                queryStrings.Add(Constants.QueryStrings.Facets, newFacetsValue);
            }

            var cleanUrl = uri.GetLeftPart(UriPartial.Path);
            var newUrl = queryStrings.Count > 0 ? $"{cleanUrl}?{queryStrings}" : cleanUrl;
            return HttpUtility.UrlEncode(newUrl);
        }
    }
}