#region

using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

#endregion

namespace CSDemo.Models.Navigation
{
    [SitecoreType(AutoMap = true)]
    public class MainNavigation
    {
        [SitecoreField]
        public virtual Image MegaMenuImage { get; set; }

        [SitecoreChildren]
        public IEnumerable<NavigationItem> NavigationItems { get; set; }
    }
}