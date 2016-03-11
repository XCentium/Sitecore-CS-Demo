#region

using System.Collections.Generic;
using CSDemo.Contracts.Navigation;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Models.Navigation
{
    [SitecoreType(AutoMap = true)]
    public class MegaMenuNavigationItem : IMegaMenuNavigationItem
    {
        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }
        public virtual string Title { get; set; }
        public virtual IEnumerable<Item> Links { get; set; }
    }
}