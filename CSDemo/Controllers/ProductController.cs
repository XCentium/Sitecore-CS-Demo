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

#endregion

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreContext _context;

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
            try
            {
                var item = RenderingContext.Current.Rendering.Item;
                var featuredProduct = item.GlassCast<FeaturedProduct>();
                if (featuredProduct?.Products != null && featuredProduct.Products.Any())
                {
                    products.AddRange(featuredProduct.Products);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return View(products);
        }
    }
}