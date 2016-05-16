using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts.Blog;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Blog
{
    public class BlogTag : IBlogTag
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }
        [SitecoreField(Fields.TagName)]
        public virtual string TagName { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string TagName = "Tag Name";
        }
        #endregion
    }
}