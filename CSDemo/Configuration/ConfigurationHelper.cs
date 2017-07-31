using System.Configuration;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;

namespace CSDemo.Configuration
{
    public static class ConfigurationHelper
    {
        public static string GetSiteSettingInfo(string suffix)
        {
            var setting = $"Site_{Context.Site.Name}_{suffix}";
            var settingValue = Settings.GetSetting(setting);
            return settingValue.IsEmptyOrNull() ? Settings.GetSetting($"Site_XCentiumCSDemo_{suffix}") : settingValue;
        }

        public static string GetSitePrefix()
        {
            return $"/sitecore/content/{Context.Site.Name}/";
        }

        public static string GetSearchIndex()
        {
            return $"sitecore_{Context.Database.Name.ToLower()}_index";
        }

        public static string GetAnayticsIndex()
        {
            return "sitecore_analytics_index";
        }

        public static string GetRecommendationsApiKey()
        {
            return ConfigurationManager.AppSettings["RecommendationsApiKey"];
        }

        public static string GetRecommendationsApiBaseUri()
        {
            return ConfigurationManager.AppSettings["RecommendationsApiBaseUri"];
        }

        public static string GetBraintreeVaultPaymentToken()
        {
            return ConfigurationManager.AppSettings["BrainTreeVaultPaymentToken"];
        }
    }
}