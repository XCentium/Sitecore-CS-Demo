#region

using System.Collections.Generic;
using CSDemo.Models.CatalogGenerated;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Models.Navigation
{
    [SitecoreType(AutoMap = true)]
    public class MegaMenuNavigationItem
    {
        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual IEnumerable<Item> Links { get; set; }
    }
}