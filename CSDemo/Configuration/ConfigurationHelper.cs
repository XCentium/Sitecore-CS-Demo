using Sitecore;
using Sitecore.Configuration;
using Sitecore.Mvc.Extensions;

namespace CSDemo.Configuration
{
    public static class ConfigurationHelper
    {
        public static string GetSiteSettingInfo(string suffix)
        {
            var catalogName = Settings.GetSetting($"Site_{Context.Site}_{suffix}");
            return catalogName.IsEmptyOrNull() ? Settings.GetSetting($"Site_XCentiumCSDemo_{suffix}") : catalogName;
        }
    }
}