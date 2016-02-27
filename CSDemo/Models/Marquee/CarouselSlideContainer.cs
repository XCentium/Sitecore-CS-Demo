using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using CSDemo.Contracts.Marquee;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Marquee {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CarouselSlideContainer : SitecoreItem, ISitecoreItem, ICarouselSlideContainer {
        
        #region Members
        public const string SitecoreItemTemplateId = "{96087765-72C4-49FB-9E49-E29BDDE87446}";
        #endregion
    }
}
