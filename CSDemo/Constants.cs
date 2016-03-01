using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo
{
    public class Constants
    {
        public const string DefaultSiteName = "Website";
        public const string DeliveryDatabase = "web";

        public struct QueryStrings
        {
            /// <summary>
            /// Used for paging
            /// </summary>
            public const string Paging = "p";

            /// <summary>
            /// Used for the sorting field
            /// </summary>
            public const string Sort = "s";

            /// <summary>
            /// Used for the sorting field direction
            /// </summary>
            public const string SortDirection = "sd";

            /// <summary>
            /// Used for facets
            /// </summary>
            public const string Facets = "f";

            /// <summary>
            /// Used for separating facets
            /// </summary>
            public const char FacetsSeparator = '|';

            public const string FacetOptionSeparator = "||";

            public const char FacetOptionDefinitionSeparator = '=';

            /// <summary>
            /// Used for the search keyword
            /// </summary>
            public const string SearchQuery = "q";

            /// <summary>
            /// Used for page size
            /// </summary>
            public const string PageSize = "ps";
        }

        public struct Commerce
        {
            public const string CatalogName = "Adventure Works Catalog";

            public const string DefaultCurrencyCode = "USD";

            public static string[] DefaultPriceTypeIds = new string[2] { "List", "Adjusted" };

            public const string Departments = "Departments";
        }

        public struct Marketing
        {
            public const string AddToCartGoalId = "{027F31D6-E67E-49C8-B7B2-C589BD62D7B0}";
            public const string PurchaseOutcomeDefinitionId = "{9016E456-95CB-42E9-AD58-997D6D77AE83}";
            public const string SubmitOrderGoalId = "{00F026E7-D067-4546-8C6F-2E67E591494A}";
        }
    }
}