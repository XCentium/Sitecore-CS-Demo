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

        public ActionResult ProductDetails()
        {
            return View();
        }

        public ActionResult CategoryListing()
        {
            CategoryListingViewModel model = new CSDemo.Models.Product.CategoryListingViewModel();


            // get the current rendering
            RenderingContext rc = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull;
            if (rc != null)
            {
                RenderingParameters rcParams = rc.Rendering.Parameters;

                string catalogueId = string.Empty;

                if (rcParams[CategorylistingConfig.TargetCatalogueFieldName] != null)
                {
                    if (!string.IsNullOrEmpty(rcParams[CategorylistingConfig.TargetCatalogueFieldName]))
                    {
                        catalogueId = rcParams[CategorylistingConfig.TargetCatalogueFieldName].ToString().Trim();

                        ChildList catalogueCategories =
                            Sitecore.Context.Database.GetItem(catalogueId)
                                .Children.AsQueryable()
                                .FirstOrDefault(x => x.Name.Equals(Constants.Commerce.Departments))
                                .GetChildren();

                        if (catalogueCategories != null)
                        {
                            model.Categories = from c in catalogueCategories
                                select c.CreateAs<GeneralCategory>();
                            return View("~/Views/Product/CategoryListings.cshtml", catalogueCategories);
                        }
                    }
                }
            }

            // get the parameter value

            return View("~/Views/Product/CategoryListings.cshtml", model);
        }

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
            var categoriesAliasId = new ID(Constants.Commerce.CategoriesAliasItemId);

            ITracker tracker = Tracker.Current;
            if (tracker == null) return products;
            if (tracker.Contact == null) return products;
            if (tracker.Interaction?.Pages == null || tracker.Interaction.Pages.Length == 0)
                return products;
            List<Page> recentPageHistory = new List<Page>();
            var currentPages = tracker.Interaction.Pages;
            if (currentPages != null && currentPages.Any())
            {
                recentPageHistory.AddRange(currentPages);
            }
            var interactionData = tracker.Contact.LoadHistorycalData(100); // configure this
            foreach (IInteractionData data in interactionData)
            {
                if(data.Pages != null && data.Pages.Any())
                    recentPageHistory.AddRange(data.Pages);
            }
            foreach (Page page in recentPageHistory.Where(t => t.Item != null && t.Item.Id != Guid.Empty && new ID(t.Item.Id) == categoriesAliasId))
            {
                ID itemId = new ID(page.Item.Id);
                if (itemId != categoriesAliasId)
                    continue;

                Item item = Sitecore.Context.Database.GetItem(itemId);
                // if this is a product the name will be "*" due to link provider resolution, so fetch it from the interaction page url
                var productName = FetchProductName(page.Url.Path); 

                // Hunt down the item from the search context
                using (
                    IProviderSearchContext searchContext =
                        ContentSearchManager.GetIndex((SitecoreIndexableItem) item).CreateSearchContext())
                {
                    SearchResultItem result = searchContext.GetQueryable<SearchResultItem>().FirstOrDefault(t => String.Equals(t.Name, productName, StringComparison.CurrentCultureIgnoreCase)); // && t.TemplateName.Contains("a matching template name")
                    if (result != null)
                    {
                        var resultItem = result.GetItem();
                        var product = resultItem.GlassCast<Product>();
                        if(!products.Any() || !products.Exists(t => t.ID == product.ID))
                            products.Add(product);
                    }
                }
                if (products.Count > _maxNumberOfProductsToShow) break;
            }
            return products;
        }

        #endregion
    }
}