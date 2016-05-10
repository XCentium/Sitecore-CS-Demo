#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Configuration;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using Newtonsoft.Json;
using CSDemo.Models.Account;

#endregion

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
        public ActionResult FeaturedProducts()
        {
            var products = new List<Product>();
            products.AddRange(GetRecentlyViewedProducts());
            if (products.Count == MaxNumberOfProductsToShow) return View(products);
            try
            {
                var item = RenderingContext.Current.Rendering.Item;
                var featuredProduct = item.GlassCast<FeaturedProducts>();
                if (featuredProduct?.Products != null && featuredProduct.Products.Any())
                {
                    foreach (var product in featuredProduct.Products)
                    {
                        if (products.Count < MaxNumberOfProductsToShow)
                        {
                            if (!products.Exists(t => t.ID == product.ID))
                                products.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return View(products);
        }

        public ActionResult FeaturedCategories()
        {
            var item = RenderingContext.Current.Rendering.Item; // csdemo >> home
            var featuredCategories = item.GlassCast<FeaturedCategories>();
            return View(featuredCategories.Categories);
        }

        public ActionResult SpecialtySelectedForYou()
        {
            var empty = new List<Product>();
            var datasource = RenderingContext.Current.Rendering.DataSource;
            if (datasource.IsEmptyOrNull()) return View(empty);
            var parentItem = _context.Database.GetItem(datasource);
            if (parentItem == null) return View(empty);
            var personalizedProducts = parentItem.GlassCast<PersonalizedProducts>();
            if (!string.IsNullOrEmpty(personalizedProducts.CouponCode)){ProductHelper.SetPersonalizedCoupon(personalizedProducts);}
            return personalizedProducts?.Products != null ? View(personalizedProducts.Products) : View(empty);
        }

        public ActionResult Categories()
        {
{}
            var model = new List<Category>();

            if (!string.IsNullOrEmpty(_userCatalogIds))
            {
                model = ProductHelper.GetCatalogCategories(_userCatalogIds);
            }

            return View(model);
        }

        public ActionResult CategoryProducts([CanBeNull] PaginationViewModel model)
        {
            var categoryProduct = new CategoryProductViewModel();

            if (model == null || string.IsNullOrEmpty(model.Category))
            {
                model = new PaginationViewModel();
                var categoryName = WebUtil.GetUrlName(0);

                var cid = Request.QueryString[Constants.Products.CategoryId];

                model.Category = categoryName;
                model.UserCatalogIds = _userCatalogIds;
                if (!string.IsNullOrEmpty(categoryName))
                {

                    var categoryId = (!string.IsNullOrEmpty(cid)) ? cid : ProductHelper.GetItemIdsFromName(categoryName, _userCatalogIds);

                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        var rc = RenderingContext.CurrentOrNull;

                        if (rc != null && rc.Rendering.Parameters[Constants.Products.PageSize] != null)
                        {
                            var pageSizeData = rc.Rendering.Parameters[Constants.Products.PageSize];
                            if (!pageSizeData.IsEmptyOrNull())
                            {
                                int pageSize = 0;
                                var success = int.TryParse(pageSizeData, out pageSize);
                                model.PageSize = 2;
                                if (success)
                                {
                                    model.PageSize = pageSize;
                                }
                            }

                        }

                        model.CategoryId = categoryId;
                        model.CurrentPage = 1;
                        model.OrderBy = string.Empty;

                        categoryProduct = ProductHelper.GetCategoryProducts(model);
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    categoryProduct = ProductHelper.GetCategoryProducts(model);
                }
            }

            return View(categoryProduct);
        }

        public ActionResult ProductDetail()
        {
            var product = GetProduct();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VisitorSignupForStockNotification(Product model)
        {
            if (ModelState.IsValid)
            {
                NotificationSigneupInput inputModel = new NotificationSigneupInput()
                {
                    CatalogName = model.CatalogName,
                    Email = model.VisitorSignupForStockNotificationEmail,
                    ProductId = model.ProductId
                };
                Product.VisitorSignupForStockNotification(Context.Site.Name, inputModel, model.LocationName ?? string.Empty);
                return Redirect(model.Url + Constants.Products.NotificationSuccess);
            }
            return Redirect(model.Url + Constants.Products.NotificationError);
        }

        #region Testing
    
        public ActionResult ProductDetailNoAvailability()
        {
            var product = GetProduct();
            return View(product);
        }

        #endregion

        #region Private Helpers

        private static IEnumerable<Product> GetRecentlyViewedProducts()
        {
            var products = new List<Product>();
            var cookie = Cookie.Get("featuredProducts");
            if (cookie != null && !cookie.Value.IsEmptyOrNull())
            {
                var ids = cookie.Value.Split(',').ToList();
                foreach (var id in ids.AsEnumerable().Reverse())
                {
                    var item = Context.Database.GetItem(ConfigurationHelper.GetSiteSettingInfo("Wildcard"));
                    SearchForProduct(item, id, products);
                    if (products.Count > MaxNumberOfProductsToShow) break;
                }
            }
            return products;
        }

        private static void SearchForProduct(Item item, string id, List<Product> products)
        {
            using (
                var searchContext =
                    ContentSearchManager.GetIndex((SitecoreIndexableItem)item).CreateSearchContext())
            {
                var result =
                    searchContext.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name)
                        .FirstOrDefault(
                            t => string.Equals(t.Name, id, StringComparison.CurrentCultureIgnoreCase));
                if (result != null)
                {
                    var resultItem = result.GetItem();
                    var product = resultItem.GlassCast<Product>();
                    if (!products.Any() || !products.Exists(t => t.ID == product.ID))
                        products.Add(product);
                }
            }
        }

        private Product GetProduct()
        {
            var categoryId = WebUtil.GetUrlName(1);
            var productId = WebUtil.GetUrlName(0).Replace(Constants.Common.Space, Constants.Common.Dash);

            if (productId.IsEmptyOrNull())
            {
                return null;
            }

            Cookie.SaveFeaturedProductCookie(productId);

            Product model = ProductHelper.GetProductByNameAndCategory(productId, categoryId);
            model.ProfileProduct(_context);

            if (model.StockInformation?.Location != null)
            {
                model.LocationName = model.StockInformation.Location.Name;
            }
            return model;
        }

        #endregion

        #region Fields

        private readonly ISitecoreContext _context;
        private const int MaxNumberOfProductsToShow = 10;
        private readonly AccountHelper _usr = new AccountHelper();
        private readonly string _userCatalogIds;

        #endregion

        #region Constructors

        public ProductController(ISitecoreContext context)
        {
            _context = context;
        }

        public ProductController()
            : this(new SitecoreContext())
        {
            _userCatalogIds = _usr.GetCurrentCustomerCatalogIds();
        }

        #endregion
    }
}