#region

using System.Collections.Generic;
using CSDemo.Contracts.Navigation;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

#endregion

namespace CSDemo.Models.Navigation
{
    [SitecoreType(AutoMap = true)]
    public class NavigationItem : INavigationItem
    {
        public struct NavigationConstants
        {
            public const string MegaMenuImage = "Mega Menu Image";
            public const string MegaMenuTitle = "Mega Menu Title";
        }
        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }
        public Link Link { get; set; }
        [SitecoreField(NavigationConstants.MegaMenuTitle)]
        public string MegaMenuTitle { get; set; }
        [SitecoreField(NavigationConstants.MegaMenuImage)]
        public virtual Image MegaMenuImage { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<MegaMenuNavigationItem> MegaMenuItems { get; set; } 
    }
}