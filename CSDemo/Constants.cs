namespace CSDemo
{
    public class Constants
    {
        public const string DefaultSiteName = "Website";
        public const string DeliveryDatabase = "web";
        public const string WebIndex = "sitecore_web_index";
        public struct QueryStrings
        {

            public const string ShowProductType = "ShowProductType";

            public const string HideProductType = "HideProductType";
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
            public const string DefaultCurrencyCode = "USD";
            public static string[] DefaultPriceTypeIds = new string[2] { "List", "Adjusted" };
            public const string Departments = "Departments";
            public const string CategoryDepartments = "categories/departments";
            public const string ProductBaseTemplateId = "{225F8638-2611-4841-9B89-19A5440A1DA1}";
            public const string CategoryBaseTemplateId = "{4C4FD207-A9F7-443D-B32A-50AA33523661}";
            public const string PurchaseOutcomeId = "{9016E456-95CB-42E9-AD58-997D6D77AE83}";
            public const string ProductTagsFieldId = "{CB2364DB-F3ED-46D2-AAC4-CFE3A280E7DD}";
            public const string ProductTagNameFieldId = "{B5620C00-330B-479E-9B54-57857040E1E0}";
            public const string DefaultSocialDomainForCommerce = "CommerceUsers";
            public const string CommerceUserDomain = "CommerceUser";
            public const string CommerceCustomerId = "scommerce_customer_id";
            public const string CommerceUserId = "user_id";
            public const string CommerceUserCatalogSetId = "user_catalog_set";
            public const string CommerceUserLoggedIn = "CommerceUserLoggedIn";
            public const string UserCatalogOptions = "UserCatalogOptions";
            public const string UserSelectedCatalogId = "UserSelectedCatalogId";
            public const string ShowCoupon = "ShowCoupon";
            public const string CouponMessage = "CouponMessage";
            public const string UserSelectedCatalogPostfix = "UserSelectedCatalogPostfix";

        }
        public struct Marketing
        {
            public const string AddToCartGoalId = "{027F31D6-E67E-49C8-B7B2-C589BD62D7B0}";
            public const string PurchaseOutcomeDefinitionId = "{9016E456-95CB-42E9-AD58-997D6D77AE83}";
            public const string SubmitOrderGoalId = "{00F026E7-D067-4546-8C6F-2E67E591494A}";
            public const string StockNotificationEngagementPlanId = "{B8249271-2721-4A1F-BBFA-9F52D5A9B5F2}";
        }
        public struct Account
        {
            public const string CustomerPhotoPath = "/sitecore/media library/CSDemo/Customers/";
            public const string FacetEmail = "Emails";
            public const string PersonalEmail = "Personal Email";
            public const string FacetPersonal = "Personal";
            public const string FacetPicture = "Picture";
            public const string JobTitle = "Customer";
            public const string SigninLink = "/account/signin";
            public const string AddressLink = "/account/Addresses";
            public const string AddressLinkItem = "/account/Addresses";
            public const string SigninErrorMsg1 = "?msg=Only Commerce Customers Allowed to Signin";
            public const string SigninErrorMsg2 = "?msg=Incorrect Username or Password";
            public const string CatalogSetField = "IncludedCatalogs";
            public const string Error = "Error";
        }
        public struct Sitecore
        {
            public const string AnalyticsIndexName = "sitecore_analytics_index";
        }
        public struct Common
        {
            public const string Dash = "-";
            public const string Underscore = "_";
            public const string ForwardSlash = "/";
            public const string Space = " ";
            public const string Comma = ",";
            public const string Empty = "";
            public const char PipeSeparator = '|';
            public const string PipeStringSeparator = "|";
            public const string True = "true";
        }
        public struct Page
        {
            public const string MetaDescription = "Meta Description";
            public const string PageTitle = "Page Title";
        }
        public struct Store
        {
            public const string GoogleLocationMatrixApiUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";
        }
        public struct Products
        {
            public const string IdSeparator = "|";
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
            public const string VariantIdFormat = "_{0}";
            public const string CurrencyFormat = "C2";
            public const string CurrencyDecimalFormat = "c";
            public const string DateFormat = "MMMM dd, yyyy hh:mm";
            public const string DateTimeFormat = "MMMM dd, yyyy hh:mm";
            public const string VariantColorName = "ProductColor";
            public const string VariantColorDisplay = "Block";
            public const string VariantColorDisplayNone = "None";
            public const string VariantColorLineFormat = "{0}|{1}|{2}";
            public const string VariantColorNameFormat = "{0}{1}";
            public const string VariantImage1 = "Image1";
            public const string VariantFirstImage = "Variant_Image1";
            public const string TrackingFieldId = "{B0A67B2A-8B07-4E0B-8809-69F751709806}";
            public const string AlsoBoughtProductsUrl = "http://xcp13n.xcentium.net/api/data/relatedproducts/csdemo/{0}";
            public const string GeoTargetedProductsUrl = "http://xcp13n.xcentium.net/api/data/geoproducts/csdemo/{0}";
            public const string AdventureWorksRootPath = "/sitecore/Commerce/Catalog Management/Catalogs/Adventure Works Catalog/Departments";
            public const string CatalogsRootPath = "/sitecore/Commerce/Catalog Management/Catalogs";
            public const string GeneralCategoryTemplateName = "GeneralCategory";
            public const string CategoryId = "cid";
            public const string NotificationSuccess = "?msg=success";
            public const string NotificationError = "?msg=error";
            public const string FeaturedProducts = "FeaturedProducts";
        }
        public struct Cart
        {
            public const string AnonUserActionDenied = "Action DENIED! Only Visitors or Signed in Customers Allowed";
            public const string BasketErrors = "_Basket_Errors";
            public const string CookieName = "_minicart";
            public const string VisitorID = "VisitorId";
            public const string VisitorTrackingCookieName = "_visitor";
            public const string VisitorIdKeyName = "visitorId";
            public const string CartNotInCache = "CartCacheHelper::GetCart - Cart for customerId {0} does not exist in the cache!";
            public const string CartInvalidInCache = "CartCacheHelper::InvalidateCartCache - Cart for customer id {0} is not in the cache!";
            public const string CartAlreadyInCache = "CartCacheHelper::AddCartToCache - Cart for customer id {0} is already in the cache!";
            public const string ErrorInBasket = "Error in Basket";
            public const string AbandonedCartSocialGoalItemId = "{D2D3D9F2-4DFE-452F-9BEF-F1E79EDD0A11}";

            public const string BillingAddressNamePrefix = "Billing_";
            public const string ShippingAddressNamePrefix = "Shipping_";
            public const string EmailAddressNamePrefix = ShippingAddressNamePrefix + "Email_";
        }
        public struct Blog
        {
            public const string Category = "category";
            public const string Tag = "tag";
            public const string BlogTemplate = "Blog Post";
            public const string TagsPath = "/sitecore/content/Data/Blogs/Tags";
            public const string AuthorsPath = "/sitecore/content/Data/Blogs/Authors";
            public const string CategoriesPath = "/sitecore/content/Data/Blogs/Categories";
        }

        public struct Social
        {
            public const string FacebookAppConfigItemId = "{98698A26-FA7E-40E4-B723-1A732B66CAF3}";
        }
    }
}