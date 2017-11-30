using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models
{
    [SitecoreType(AutoMap = true)]
    public class ProgramModel : IEditableBase
    {
        [SitecoreField("Quarterly Order Weight Limit")]
        public virtual double QuarterlyOrderWeightLimit { get; set; }

        [SitecoreField("Quarterly Order Price Limit")]
        public virtual decimal QuarterlyOrderPriceLimit { get; set; }

        public Guid ID { get; set; }
        public string Path { get; set; }
    }
}