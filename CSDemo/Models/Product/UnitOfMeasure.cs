using System;
using System.Collections.Generic;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

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