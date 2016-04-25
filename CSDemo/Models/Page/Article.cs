using CSDemo.Contracts.Page;
using CSDemo.Contracts;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;

namespace CSDemo.Models.Page
{
    [SitecoreType(AutoMap =true, Cachable = false)]
    public class Article :IArticle, IEditableBase
    {
        public virtual string Title { get; set; }

        public virtual string Content { get; set; }

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

    }
}