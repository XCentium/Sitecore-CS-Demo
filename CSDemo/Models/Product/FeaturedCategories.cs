using System.Collections.Generic;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class FeaturedCategories : IFeaturedCategories
    {
        public virtual IEnumerable<GeneralCategory> Categories { get; set; }
    }
}