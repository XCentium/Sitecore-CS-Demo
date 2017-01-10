using CSDemo.Contracts;
using CSDemo.Contracts.Article;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;

namespace CSDemo.Models.Article
{

    [SitecoreType(AutoMap = true, Cachable = false)]
    public class AccordionData : IAccordionData, IEditableBase
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        [SitecoreField(Fields.Body)]
        public virtual string Body { get; set; }

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
            public const string Body = "Body";
            public const string Title = "Title";
        }
        #endregion


    }
}