using CSDemo.Contracts;
using CSDemo.Contracts.Article;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;

namespace CSDemo.Models.Article
{


    [SitecoreType(AutoMap = true, Cachable = false)]
    public class ArticleLinksData : IArticleLinksData, IEditableBase
    {

        #region Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        [SitecoreField(Fields.ArticlePath)]
        public Link ArticlePath { get; set; }

        [SitecoreField(Fields.LinkImage)]
        public Image LinkImage { get; set; }

        [SitecoreField(Fields.LinkText)]
        public virtual string LinkText { get; set; }

        [SitecoreField(Fields.LinkTitle)]
        public virtual string LinkTitle { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }


        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        #endregion

        #region Fields
        public struct Fields
        {
            public const string ArticlePath = "Article path";
            public const string LinkImage = "Link Image";
            public const string LinkText = "Link Text";
            public const string LinkTitle = "Link Title";
        }
        #endregion


    }
}