using System;
using System.Collections.Generic;
using CSDemo.Contracts.Blog;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Models.Blog
{
    public class Blog : IBlog
    {

        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        [SitecoreField(Fields.Category)]
        public IEnumerable<BlogCategory> Category { get; set; }

        [SitecoreField(Fields.Thumb)]
        public virtual Image Thumb { get; set; }
        [SitecoreField(Fields.Author)]
        public IEnumerable<BlogAuthor> Author { get; set; }
        [SitecoreField(Fields.Main)]
        public virtual Image Main { get; set; }
        [SitecoreField(Fields.Body)]
        public virtual string Body { get; set; }
        [SitecoreField(Fields.Tags)]
        public IEnumerable<BlogTag> Tags { get; set; }
        [SitecoreField(Fields.Summary)]
        public virtual string Summary { get; set; }
        [SitecoreField(Fields.PublishDate)]
        public virtual DateTime PublishDate { get; set; }
        [SitecoreField(Fields.Title)]
        public virtual string Title { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string Category = "Category";
            public const string Thumb = "Thumb";
            public const string Author = "Author";
            public const string Main = "Main";
            public const string Body = "Body";
            public const string Tags = "Tags";
            public const string Summary = "Summary";
            public const string PublishDate = "Publish Date";
            public const string Title = "Title";
        }
        #endregion

    }
}