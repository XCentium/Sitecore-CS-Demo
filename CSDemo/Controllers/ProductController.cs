#region

using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Models.CatalogGenerated;
using CSDemo.Models.Parameters;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using XCore.Framework;
using CSDemo.Contracts.Product;
using Sitecore.Analytics;
using Sitecore.Analytics.Core;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Extensions;

#endregion

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
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

        public ActionResult FeaturedProducts()
        {
            List<Product> products = new List<Product>();
            products.AddRange(GetRecentlyViewedProducts());
            if (products.Count == _maxNumberOfProductsToShow) return View(products);
            try
            {
                Item item = RenderingContext.Current.Rendering.Item;
                FeaturedProduct featuredProduct = item.GlassCast<FeaturedProduct>();
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

        #region Private Helpers

        private static string FetchProductName(string interactionPageUrl)
        {
            var categoriesIndex = interactionPageUrl.ToLower().IndexOf("categories", StringComparison.Ordinal);
            var lastSlashIndex = interactionPageUrl.LastIndexOf("/", StringComparison.Ordinal);
            if (lastSlashIndex > categoriesIndex && interactionPageUrl.Length > lastSlashIndex)
            {
                var productName = interactionPageUrl.Substring(lastSlashIndex + 1);
                if(!productName.IsEmptyOrNull())
                    return productName;
            }
            return string.Empty;
        }

        private static IEnumerable<Product> GetRecentlyViewedProducts()
        {
            List<Product> products = new List<Product>();
            var cookie = Cookie.Get("featuredProducts");
            if (cookie != null && !cookie.Value.IsEmptyOrNull())
            {
                var ids = cookie.Value.Split(',').ToList();
                foreach (var id in ids)
                {
                    Item item = Sitecore.Context.Database.GetItem(Constants.Commerce.CategoriesAliasItemId);
                    using (
                    IProviderSearchContext searchContext =
                        ContentSearchManager.GetIndex((SitecoreIndexableItem)item).CreateSearchContext())
                    {
                        SearchResultItem result = searchContext.GetQueryable<SearchResultItem>().FirstOrDefault(t => String.Equals(t.Name, id, StringComparison.CurrentCultureIgnoreCase));
                        if (result != null)
                        {
                            var resultItem = result.GetItem();
                            var product = resultItem.GlassCast<Product>();
                            if (!products.Any() || !products.Exists(t => t.ID == product.ID))
                                products.Add(product);
                        }
                    }
                    if (products.Count > _maxNumberOfProductsToShow) break;
                }
            }
            return products;
        }

        #endregion
    }
}