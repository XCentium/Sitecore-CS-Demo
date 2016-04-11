using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(TemplateId = "{6DE0426C-E5D4-4E7B-879D-2B88DA56771A}", AutoMap = true)]
    public class UnitOfMeasure : IUnitOfMeasure
    {
        public virtual string Type { get; set; }

        [SitecoreChildren(InferType = true)]
        public virtual IEnumerable<UnitOfMeasureEnumeration> Enumerations { get; set; }
    }
}