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
            var catalogName = Settings.GetSetting(setting);
            return catalogName.IsEmptyOrNull() ? Settings.GetSetting($"Site_XCentiumCSDemo_{suffix}") : catalogName;
        }

        public static string GetSitePrefix()
        {
            return $"/sitecore/content/{Sitecore.Context.Site.Name}/";
        }
    }
}