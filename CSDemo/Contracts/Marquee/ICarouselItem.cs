using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Contracts.Marquee
{
    public partial interface ICarouselItem : ISitecoreItem
    {
        #region Properties
        ImageField Image { get; set; }

        string Content { get; set; }
        #endregion

    }
}