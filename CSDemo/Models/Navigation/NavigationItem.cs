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
    public class NavigationItem
    {
        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<MegaMenuNavigationItem> MegaMenuNavigationItems { get; set; }


    }
}