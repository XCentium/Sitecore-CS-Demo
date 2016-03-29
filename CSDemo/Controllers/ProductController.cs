#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Configuration;
using CSDemo.Models;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Data.DataAccess.VisitorCache;
using Sitecore.Commerce.Connect.CommerceServer.Catalog.Fields;
using Sitecore.Commerce.Connect.CommerceServer.Controls;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;
using Sitecore.Web;
using XCore.Framework;

using Sitecore.Analytics;
using Sitecore.Analytics.Automation;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Automation.MarketingAutomation;
using Sitecore.Commerce.Contacts;

#endregion

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
        public ActionResult FeaturedProducts()
        {
            var products = new List<Product>();
            products.AddRange(GetRecentlyViewedProducts());
            if (products.Count == _maxNumberOfProductsToShow) return View(products);
            try
            {
                var item = RenderingContext.Current.Rendering.Item;
                var featuredProduct = item.GlassCast<FeaturedProducts>();
                if (featuredProduct?.Products != null && featuredProduct.Products.Any())
                {
                    foreach (var product in featuredProduct.Products)
                    {
                        if (products.Count < _maxNumberOfProductsToShow)
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
            return personalizedProducts?.Products != null ? View(personalizedProducts.Products) : View(empty);
        }

        public ActionResult Categories()
        {
            var model = new List<Category>();

            var rc = RenderingContext.CurrentOrNull;

            if (rc != null)
            {
                var rcParams = rc.Rendering.Parameters;

                var categoryIDs = rcParams[Constants.Products.ParameterKey];

                if (categoryIDs != null)
                {
                    if (!string.IsNullOrEmpty(categoryIDs))
                    {
                        model = ProductHelper.GetCategories(categoryIDs);
                    }
                }
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
                model.Category = categoryName;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    var categoryID = ProductHelper.GetItemIDFromName(categoryName, Constants.Products.CategoriesParentId);

                    if (!string.IsNullOrEmpty(categoryID))
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

                        model.CategoryID = categoryID;
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
            var categoryID = WebUtil.GetUrlName(1);
            var productID = WebUtil.GetUrlName(0).Replace(Constants.Common.Space, Constants.Common.Dash);

            if (productID.IsEmptyOrNull())
            {
                return View();
            }

            // CSDEMO#49 Save featured products
            Cookie.SaveFeaturedProductCookie(productID);

            Product model = ProductHelper.GetProductByNameAndCategory(productID, categoryID);

            // CSDEMO#115
            model.ProfileProduct(_context);

            // CSDEMO#116
            if (model.StockInformation?.Location != null)
            {
                model.LocationName = model.StockInformation.Location.Name;
            }

            return View(model);
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
                return Redirect(model.Url + "?msg=error");
            }
            return Redirect(model.Url + "?msg=success");
        }

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
                    var item = Context.Database.GetItem(Constants.Commerce.CategoriesAliasItemId);
                    SearchForProduct(item, id, products);
                    if (products.Count > _maxNumberOfProductsToShow) break;
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

        #endregion

        #region Fields

        private readonly ISitecoreContext _context;
        private const int _maxNumberOfProductsToShow = 10;

        #endregion

        #region Constructors

        public ProductController(ISitecoreContext context)
        {
            _context = context;
        }

        public ProductController()
            : this(new SitecoreContext())
        {
        }

        #endregion
    }
}