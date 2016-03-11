using System;
using System.Collections.Generic;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class GeneralCategory : IGeneralCategory
    {
        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; } 

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Brand { get; set; }

        [SitecoreField]
        public virtual string Description { get; set; }

        [SitecoreField]
        public virtual IEnumerable<Image> Images { get; set; } 
    }
}