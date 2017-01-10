using CSDemo.Contracts;
using CSDemo.Contracts.Article;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;

namespace CSDemo.Models.Article
{


    [SitecoreType(AutoMap = true, Cachable = false)]
    public class ArticlePage : IArticlePage, IEditableBase
    {
        public virtual string Title { get; set; }

        public virtual string Content { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        public virtual string Url { get; set; }

        [SitecoreField(Fields.HeaderImage)]
        public virtual Image HeaderImage { get; set; }

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreField(Fields.Articlelink)]
        public virtual ArticleLinks Articlelink { get; set; }

        [SitecoreField(Fields.Accordion)]
        public virtual Accordion Accordion { get; set; }

        [SitecoreField(Fields.Caption)]
        public virtual string Caption { get; set; }

        #region Fields

        public struct Fields
        {
            public const string HeaderImage = "Header Image";
            public const string Articlelink = "Article link";
            public const string Accordion = "Accordion";
            public const string Caption = "Caption";
        }

        #endregion

    }
}