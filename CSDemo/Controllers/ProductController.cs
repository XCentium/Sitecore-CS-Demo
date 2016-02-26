#region

using System.Linq;
using System.Web.Mvc;
using CSDemo.Models.CatalogGenerated;
using CSDemo.Models.Parameters;
using Sitecore.Mvc.Controllers;
using XCore.Framework;

#endregion

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
    }
}