#region

using CSDemo.Contracts.Marquee;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

#endregion

namespace CSDemo.Models.Marquee
{
    [SitecoreType(AutoMap = true)]
    public class CarouselItem : ICarouselItem
    {
        [SitecoreField]
        public Image Image { get; set; }

        [SitecoreField]
        public string Content { get; set; }
    }
}