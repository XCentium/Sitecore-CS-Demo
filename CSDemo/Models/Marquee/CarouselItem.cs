#region

using System;
using CSDemo.Contracts;
using CSDemo.Contracts.Marquee;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Configuration;

#endregion

namespace CSDemo.Models.Marquee
{
    [SitecoreType(AutoMap = true)]
    public class CarouselItem : ICarouselItem, IEditableBase
    {
        public virtual Image Image { get; set; }
        public virtual string Content { get; set; }
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }
    }
}