using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class GeoTargetedProducts
    {
        public virtual string Title { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
    }
}