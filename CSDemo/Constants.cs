using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo
{
    public class Constants
    {
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

            /// <summary>
            /// Used for the search keyword
            /// </summary>
            public const string SearchQuery = "q";

            /// <summary>
            /// Used for page size
            /// </summary>
            public const string PageSize = "ps";
        }
    }
}