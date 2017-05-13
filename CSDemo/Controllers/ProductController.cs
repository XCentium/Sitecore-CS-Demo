#region

using CSDemo.Configuration;
using CSDemo.Models.Account;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Helpers;
using Sitecore.Data;

#endregion

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
        public ActionResult FeaturedProducts()
        {
            Sitecore.Diagnostics.Log.Info("CS DEMO: Starting Featured products.", this);
            var products = new List<Product>();
            products.AddRange(GetRecentlyViewedProducts());
            if (products.Count == MaxNumberOfProductsToShow) return View(products);
            try
            {
                var item = RenderingContext.Current.Rendering.Item;
                var featuredProduct = GlassHelper.Cast<FeaturedProducts>(item);

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
                Sitecore.Diagnostics.Log.Error(ex.Message, ex);
            }

            return View(products);
        }

        public ActionResult ProductSampler()
        {
            Sitecore.Diagnostics.Log.Info("FSS DEMO: Starting Product Sampler.", this);

            try
            {
                var datasourceId = RenderingContext.Current.Rendering.DataSource;
                if (string.IsNullOrWhiteSpace(datasourceId)) throw new ArgumentException("Product Sampler Datasource is not set.");

                var productSamplerFolder = Context.Database.GetItem(datasourceId);
                var productSamplers = productSamplerFolder.Children.Select(GlassHelper.Cast<ProductSampler>).ToList();
                
                return View(productSamplers);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex);
            }

            return View();
        }

        public ActionResult DealerOrderingSystem()
        {
            return View();
        }

        public ActionResult SalesQuote()
        {
            return View();
        }


        public ActionResult QuickOrderSystem()
        {

            return View();
        }

        public ActionResult FeaturedCategories()
        {
            var item = RenderingContext.Current.Rendering.Item; // csdemo >> home
            var featuredCategories = GlassHelper.Cast<FeaturedCategories>(item);

            try
            {
                return View(featuredCategories.Categories);
            }
            catch (Exception)
            {
                return View(new List<GeneralCategory>());
            }
        }

        public ActionResult SpecialtySelectedForYou()
        {
            var empty = new List<Product>();
            var datasource = RenderingContext.Current.Rendering.DataSource;

            if (datasource.IsEmptyOrNull()) return View(empty);
            var parentItem = _context.Database.GetItem(datasource);

            if (parentItem == null) return View(empty);
            var personalizedProducts = GlassHelper.Cast<PersonalizedProducts>(parentItem);

            try
            {
                if (!string.IsNullOrEmpty(personalizedProducts.CouponCode))
                {
                    ProductHelper.SetPersonalizedCoupon(personalizedProducts);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return personalizedProducts?.Products != null ? View(personalizedProducts.Products) : View(empty);
        }

        public ActionResult GeoTargetedProducts()
        {
            Sitecore.Diagnostics.Log.Info("CS DEMO: Starting Geo Targeted products.", this);
            var geoProducts = new GeoTargetedProducts();

            try
            {
                var datasourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
                var item = ID.IsID(datasourceId) ? Context.Database.GetItem(datasourceId) : null;

                if (item == null)
                {
                    geoProducts.Products =
                        Product.GetGeoTargetedProducts(Tracker.Current.Interaction.GeoData.AreaCode).ToList();
                    geoProducts.Title = "Based On Your Location You Might Be Interested In";

                    return View(geoProducts);
                }

                geoProducts = GlassHelper.Cast<GeoTargetedProducts>(item);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex);
            }

            return View(geoProducts);
        }

        public ActionResult Categories()
        {
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

            if (string.IsNullOrEmpty(model?.Category))
            {
                model = new PaginationViewModel();

                var categoryName = WebUtil.GetUrlName(0);
                var cid = Request.QueryString[Constants.Products.CategoryId];

                model.Category = categoryName;
                model.UserCatalogIds = _userCatalogIds;

                if (!string.IsNullOrEmpty(categoryName))
                {

                    if (_catalogPostFix != null)
                    {
                        if (!categoryName.ToLower().Contains(_catalogPostFix.ToLower()))
                        {
                            categoryProduct = GetCategoryProducts(cid, categoryName + _catalogPostFix);

                            if (categoryProduct != null && categoryProduct.PaginationViewModel.TotalItems > 0)
                            {
                                return View(categoryProduct);
                            }
                        }
                    }

                    categoryProduct = GetCategoryProducts(cid, categoryName);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.UserCatalogIds = _userCatalogIds;
                    categoryProduct = ProductHelper.GetCategoryProducts(model);
                }
            }

            return View(categoryProduct);
        }

        private CategoryProductViewModel GetCategoryProducts(string cid, string categoryName)
        {
            var model = new PaginationViewModel
            {
                Category = categoryName,
                UserCatalogIds = _userCatalogIds
            };

            if (!string.IsNullOrEmpty(categoryName))
            {


                var categoryId = (!string.IsNullOrEmpty(cid)) ? cid : ProductHelper.GetItemIdsFromName(categoryName, _userCatalogIds);
                if (categoryId == "") { categoryId = (!string.IsNullOrEmpty(cid)) ? cid : ProductHelper.GetItemIdsFromName(categoryName.Replace(" ", "-"), _userCatalogIds); }

                if (!string.IsNullOrEmpty(categoryId))
                {
                    var rc = RenderingContext.CurrentOrNull;

                    if (rc?.Rendering.Parameters[Constants.Products.PageSize] != null)
                    {
                        var pageSizeData = rc.Rendering.Parameters[Constants.Products.PageSize];
                        if (!pageSizeData.IsEmptyOrNull())
                        {
                            int pageSize;
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

                    return ProductHelper.GetCategoryProducts(model, false);
                }
            }

            return null;
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
                var inputModel = new NotificationSigneupInput()
                {
                    CatalogName = model.CatalogName,
                    Email = Context.User.Profile.Email,
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
            var cookie = Cookie.Get(Constants.Products.FeaturedProducts);

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
                    var addProd = true;
                    var resultItem = result.GetItem();
                    if (_catalogPostFix != null)
                    {
                        if (!resultItem.Name.ToLower().Contains(_catalogPostFix.ToLower()))
                        {
                            var tmpProduct = ProductHelper.GetProductByNameAndCategory(resultItem.Name + _catalogPostFix, _catalogPostFix);
                            if (tmpProduct?.DefinitionName != null)
                            {
                                products.Add(tmpProduct);
                                addProd = false;
                            }
                        }
                    }

                    if (!addProd) return;

                    var product = GlassHelper.Cast<Product>(resultItem);
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

            if (_catalogPostFix != null)
            {
                if (!productId.ToLower().Contains("("))
                {
                    productId = productId + _catalogPostFix;

                    if (!categoryId.ToLower().Contains("("))
                    {
                        categoryId = categoryId + _catalogPostFix;
                    }

                    Response.Redirect($"/categories/{categoryId}/{productId}");
                    Response.End();
                }
            }

            Cookie.SaveFeaturedProductCookie(productId);

            var model = ProductHelper.GetProductByNameAndCategory(productId, categoryId);
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
        private static string _catalogPostFix;

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
            _catalogPostFix = _usr.GetCurrentCustomerCatalogPostFix();
        }

        #endregion
    }
}