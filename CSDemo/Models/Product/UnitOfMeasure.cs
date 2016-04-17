using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class UnitOfMeasure : IUnitOfMeasure
    {
        public virtual Guid ID { get; set; }
        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<UnitOfMeasureEnumeration> Units { get; set; }
    }
}