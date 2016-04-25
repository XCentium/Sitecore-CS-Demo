using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace CSDemo.Configuration
{
    public class CustomLinkProvider : LinkProvider
    {
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            if (item == null) return base.GetItemUrl(item, options);
            if (item.Template.BaseTemplates.Any(t => t.ID == new ID(Constants.Commerce.ProductBaseTemplateId)))
            {
                var categoryUrl = GetCategoryUrl(item.Parent, options);
                return $"{categoryUrl}/{(options.LowercaseUrls ? item.Name.ToLower() : item.Name)}";
            }

            if (item.Template.BaseTemplates.Any(t => t.ID == new ID(Constants.Commerce.CategoryBaseTemplateId)))
            {
                return GetCategoryUrl(item, options);
            }

            return base.GetItemUrl(item, options);
        }

        #region Private Helpers

        private string GetCategoryUrl(Item categoryItem, UrlOptions options)
        {
            if(Context.Database == null) return base.GetItemUrl(categoryItem, options);
            var categoriesListingPage = Context.Database.GetItem(ConfigurationHelper.GetSiteSettingInfo("CategoryListing"));
            if (categoriesListingPage == null) return base.GetItemUrl(categoryItem, options);
            return
                $"{LinkManager.GetItemUrl(categoriesListingPage)}/{(options.LowercaseUrls ? categoryItem.Name.ToLower() : categoryItem.Name)}";
        }

        #endregion
    }
}