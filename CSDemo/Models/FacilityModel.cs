using System.Collections.Generic;
using CSDemo.Models.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models
{
    [SitecoreType(AutoMap = true)]
    public class FacilityModel
    {
        [SitecoreField("Product Category Blacklist")]
        public virtual IEnumerable<Category> BlackListedProductCategories { get; set; }
    }
}