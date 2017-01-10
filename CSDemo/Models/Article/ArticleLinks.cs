using CSDemo.Contracts;
using CSDemo.Contracts.Article;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace CSDemo.Models.Article
{

    [SitecoreType(AutoMap = true, Cachable = false)]
    public class ArticleLinks : IArticleLinks, IEditableBase
    {

        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        [SitecoreField(Fields.ArticlelinksList)]
        public IEnumerable<ArticleLinksData> ArticlelinksList { get; set; }

        [SitecoreField(Fields.Title)]
        public virtual string Title { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }
        #endregion


        #region Fields
        public struct Fields
        {
            public const string ArticlelinksList = "Article links";
            public const string Title = "Title";
        }
        #endregion

    }
}