using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class FeaturedProducts
    {
        public virtual IEnumerable<Product> Products { get; set; }
    }
}