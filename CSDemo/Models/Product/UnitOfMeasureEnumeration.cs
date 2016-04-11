using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class UnitOfMeasureEnumeration : IUnitOfMeasureEnumeration
    {
        public virtual Guid ID { get; set; }
        public virtual string Value { get; set; }
    }
}