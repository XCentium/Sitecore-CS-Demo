using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class UnitOfMeasureEnumeration
    {
        public virtual string Value { get; set; }
    }

    [SitecoreType(AutoMap = true)]
    public class UnitOfMeasure
    {
        [SitecoreChildren]
        public virtual IEnumerable<UnitOfMeasureEnumeration> Enumerations { get; set; }
    }
}