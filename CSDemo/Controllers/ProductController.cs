using System;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using CSDemo.Models;
using CSDemo.Models.Product;
using Sitecore.Mvc.Controllers;
using XCore.Framework;
using Sitecore.Commerce.Connect.CommerceServer;
using CSDemo.Models.Parameters;
using CSDemo.Configuration;
using CSDemo.Models.Product;
using CSDemo.Models.CatalogGenerated;


namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
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

                        var catalogueCategories = Sitecore.Context.Database.GetItem(catalogueId).Children.AsQueryable().FirstOrDefault(x => x.Name.Equals(Category.Departments)).GetChildren();

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
    }
}