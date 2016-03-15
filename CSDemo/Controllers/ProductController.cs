#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Models;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Sitecore;
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
using Sitecore.Web;

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
                        model.CategoryID = categoryID;
                        model.CurrentPage = 1;
                        model.PageSize = 15;
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

        /// <summary>
        /// CSDEMO#89
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<Product> FetchRelatedProducts(Guid id)
        {
            List<Product> relatedProducts = new List<Product>();
            var contextProductItem = Context.Database.GetItem(new ID(id));
            if(contextProductItem == null) return relatedProducts;
            RelationshipField control = contextProductItem.Fields[Product.Fields.RelationshipList];
            if(control == null) return relatedProducts;
            IEnumerable<Item> productRelationshipTargets = control.GetProductRelationshipsTargets();
            IEnumerable<Item> relationshipTargets = productRelationshipTargets as Item[] ?? productRelationshipTargets.ToArray();
            if (productRelationshipTargets == null || !relationshipTargets.Any()) return relatedProducts;
            relatedProducts.AddRange(relationshipTargets.Select(t => t.GlassCast<Product>()).Where(t => t != null));
            return relatedProducts;
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

            var model = ProductHelper.GetProductByNameAndCategory(productID, categoryID);
            // CSDEMO#89 Add related products from product relationships
            model.RelatedProducts = FetchRelatedProducts(model.ID);
            return View(model);

            
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
                    ContentSearchManager.GetIndex((SitecoreIndexableItem) item).CreateSearchContext())
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

        public ProductController() : this(new SitecoreContext())
        {
        }

        #endregion
    }
}