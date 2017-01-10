using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Configuration;

namespace CSDemo.Configuration
{
    public class CustomLinkProvider : LinkProvider
    {
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            // Ola added
            try
            {
                if (item == null) return base.GetItemUrl(item, options);
                if (item.Paths.Path.ToLower().Contains("/sitecore/commerce/catalog"))
                {
                    if (item.TemplateName.ToLower() == Constants.Products.GeneralCategoryTemplateName.ToLower()){
                        return "/categories/" + item.Name.ToLower();
                    }

                    if (!item.TemplateName.ToLower().Contains("variant"))
                    {
                        return "/categories/" + item.Parent.Name.ToLower() + "/" + item.Name.ToLower();
                    }

                }



                return base.GetItemUrl(item, options);
            }
            catch (System.Exception ex)
            {

                Sitecore.Diagnostics.Log.Error("Error", ex, this);
                return string.Empty;
            }

            //try
            //{
            //    if (item == null) return base.GetItemUrl(item, options);
            //    if (item.Template.BaseTemplates.Any(t => t.ID == new ID(Constants.Commerce.ProductBaseTemplateId)))
            //    {
            //        var categoryUrl = GetCategoryUrl(item.Parent, options);
            //        return $"{categoryUrl}/{(options.LowercaseUrls ? item.Name.ToLower() : item.Name)}";
            //    }

            //    if (item.Template.BaseTemplates.Any(t => t.ID == new ID(Constants.Commerce.CategoryBaseTemplateId)))
            //    {
            //        return GetCategoryUrl(item, options);
            //    }

            //    return base.GetItemUrl(item, options);
            //}
            //catch (System.Exception ex)
            //{
            //    Sitecore.Diagnostics.Log.Error("Error", ex, this);
            //    return string.Empty;

            //}
        }

        #region Private Helpers

        private string GetCategoryUrl(Item categoryItem, UrlOptions options)
        {
            var database = Context.Database;
            if (database == null || database.Name.ToLower() == "core")
                database = Factory.GetDatabase("master");
            if(database == null) return base.GetItemUrl(categoryItem, options);

            var categoriesListingPage = database.GetItem(ConfigurationHelper.GetSiteSettingInfo("CategoryListing"));
            if (categoriesListingPage == null) return base.GetItemUrl(categoryItem, options);
            return
                $"{LinkManager.GetItemUrl(categoriesListingPage)}/{(options.LowercaseUrls ? categoryItem.Name.ToLower() : categoryItem.Name)}";
        }

        #endregion
    }
}