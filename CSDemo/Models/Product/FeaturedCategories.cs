using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class FeaturedCategories : IFeaturedCategories
    {
        public virtual IEnumerable<GeneralCategory> Categories { get; set; }
    }
}