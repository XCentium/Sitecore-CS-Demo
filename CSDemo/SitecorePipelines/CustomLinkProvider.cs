using Glass.Mapper.Sc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.SitecorePipelines
{
    public class CustomLinkProvider : LinkProvider
    {
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            if (item == null) return base.GetItemUrl(item, options);
            if(item.Template.BaseTemplates.Any(t=>t.ID== new ID(Constants.Commerce.ProductBaseTemplateId)))
            {
                var categoryUrl = GetCategoryUrl(item, options);
                return $"{categoryUrl}/{(options.LowercaseUrls ? item.Name.ToLower() : item.Name)}";
            }

            if (item.Template.BaseTemplates.Any(t => t.ID == new ID(Constants.Commerce.CategoryBaseTemplateId)))
            {
                return GetCategoryUrl(item, options);
            }
         
            return base.GetItemUrl(item, options);
        }

        #region Private Helpers

        private string GetCategoryUrl(Item item, UrlOptions options)
        {
            var categoriesListingPage = Sitecore.Context.Database.GetItem(Constants.Pages.CategoriesListingPageId);
            if (categoriesListingPage == null) return base.GetItemUrl(item, options);
            return $"{LinkManager.GetItemUrl(categoriesListingPage)}/{(options.LowercaseUrls ? categoriesListingPage.Name.ToLower() : categoriesListingPage.Name)}";
        }

        #endregion
    }
}