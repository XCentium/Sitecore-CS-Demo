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
        public const string WebIndex = "sitecore_web_index";

        public struct QueryStrings
        {
            /// <summary>
            /// Used for paging
            /// </summary>
            public const string Paging = "pn";

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

            public const string ProductBaseTemplateId = "{225F8638-2611-4841-9B89-19A5440A1DA1}";
            public const string CategoryBaseTemplateId = "{4C4FD207-A9F7-443D-B32A-50AA33523661}";
            public const string CategoriesAliasItemId = "{9FB913A0-E51D-426B-9AC3-F99FD71BA08C}";
        }

        public struct Marketing
        {
            public const string AddToCartGoalId = "{027F31D6-E67E-49C8-B7B2-C589BD62D7B0}";
            public const string PurchaseOutcomeDefinitionId = "{9016E456-95CB-42E9-AD58-997D6D77AE83}";
            public const string SubmitOrderGoalId = "{00F026E7-D067-4546-8C6F-2E67E591494A}";
        }

        public struct Pages
        {
            public const string ProductDetailPageId = "{94377B9C-75CE-4D60-AB6F-4CF627E0A8EC}";
            public const string CategoriesListingPageId = "{3C5BBD6A-528F-4807-BAF0-20F238F16C0A}";
        }

        public struct Sitecore
        {
            public const string AnalyticsIndexName = "sitecore_analytics_index";
        }

        public struct Common
        {
            public const string Dash = "-";
            public const string Underscore = "_";
            public const string Space = " ";
            public const string Comma = ",";
            public const string Empty = "";

        }

        public struct Products
        {
            public const string IDSeparator = "|";
            public const string ParameterKey = "Product Categories";
            public const string PageSize = "PageSize";
            public const string ImagesField = "Images";
            public const string ImagesUrlFormat = "/~/media/{0}.ashx";
            public const string CategoryUrlFormat = "categories/{0}";
            public const string CategoriesParentId = "{4441D0B5-1080-4550-A91A-4C2C8245C986}";
            public const string CategoriesTemplateId = "{C118EAAE-D723-4560-ABFC-917E58F46F18}";
            public const string Title = "Title";
            public const string OrderByBrand = "Brand";
            public const string OrderByRatings = "Average rating";
            public const string OrderByNewness = "Newness";
            public const string OrderByPriceAsc = "Price: low to high";
            public const string OrderByPriceDesc = "Price: high to low";
            public const string BillingAddress = "Billing";
            public const string ShippingAddress = "Shipping";
            public const string VariantIDFormat = "_{0}";
            public const string CurrencyFormat = "C2";
            public const string CurrencyDecimalFormat = "c";
            public const string DateFormat = "MMMM dd, yyyy hh:mm";
            public const string DateTimeFormat = "MMMM dd, yyyy hh:mm";
            public const string VariantColorName = "ProductColor";
            public const string VariantColorDisplay = "Block";
            public const string VariantColorDisplayNone = "None";
            public const string VariantColorLineFormat = "{0}|{1}|{2}";
            public const string VariantColorNameFormat = "{0}{1}";
            public const string TrackingFieldId = "{B0A67B2A-8B07-4E0B-8809-69F751709806}";
        }

    }
}