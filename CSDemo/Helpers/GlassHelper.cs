using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Data.Items;

namespace CSDemo.Helpers
{
    public class GlassHelper
    {
        private static SitecoreService _sitecoreService;

        private static SitecoreService SitecoreService => _sitecoreService ?? (_sitecoreService = new SitecoreService(Context.Database.Name.ToLower()));

        public static T Cast<T>(Item item) where T: class 
        {
            return SitecoreService.Cast<T>(item);
        }
    }
}