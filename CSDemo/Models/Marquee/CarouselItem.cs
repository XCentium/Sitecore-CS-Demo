using System;
using System.Collections.Generic;
using CSDemo.Contracts.Marquee;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Marquee
{


    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CarouselItem : SitecoreItem, ISitecoreItem, ICarouselItem
    {

        #region Members
        public const string SitecoreItemTemplateId = "{6D36DF55-60E5-4591-899C-EFCCAF83579A}";

        public const string ImageFieldId = "{35D787AE-0F1F-412A-A570-D5A79018EBB8}";

        public const string ImageFieldName = "Image";

        public const string ContentFieldId = "{8EF3659B-8284-4484-9B7E-6FFF97496411}";

        public const string ContentFieldName = "Content";
        #endregion

        #region Properties
        [SitecoreItemField(ImageFieldId)]
        public virtual ImageField Image { get; set; }

        [SitecoreItemField(ContentFieldId)]
        public virtual string Content { get; set; }
        #endregion

    }
}
