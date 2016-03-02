#region

using System;
using System.Collections.Generic;
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
            var model = new CSDemo.Models.Product.CategoryListingViewModel();


            // get the current rendering
            var rc = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull;
            if (rc != null)
            {
                var rcParams = rc.Rendering.Parameters;

                var catalogueId = string.Empty;

                if (rcParams[CategorylistingConfig.TargetCatalogueFieldName] != null)
                {
                    if (!string.IsNullOrEmpty(rcParams[CategorylistingConfig.TargetCatalogueFieldName]))
                    {
                        catalogueId = rcParams[CategorylistingConfig.TargetCatalogueFieldName].ToString().Trim();

                        var catalogueCategories =
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
                var item = RenderingContext.Current.Rendering.Item;
                var featuredProduct = item.GlassCast<FeaturedProduct>();
                if (featuredProduct?.Products != null && featuredProduct.Products.Any())
                {
                    products.AddRange(featuredProduct.Products.Take(_maxNumberOfProductsToShow- products.Count));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return View(products);
        }

        #region Private Helpers

        private static IEnumerable<Product> GetRecentlyViewedProducts()
        {
            var products = new List<Product>();
            var tracker = Tracker.Current;
            if (tracker == null) return products;
            if (tracker.Contact == null) return products;

            if (tracker.Interaction == null || tracker.Interaction.Pages == null || tracker.Interaction.Pages.Length == 0)
                return products;

            foreach (var page in tracker.Interaction.Pages)
            {
                if (page.Item == null) continue;
                // we need to get the URL out of the page.Item and lookup the product by the last part of the URL

                if (products.Count > _maxNumberOfProductsToShow) break;
            }
            return products;
        }

        #endregion
    }
}