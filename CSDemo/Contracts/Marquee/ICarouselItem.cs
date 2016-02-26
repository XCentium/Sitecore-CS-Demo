using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper;

namespace CSDemo.Contracts.Marquee
{
    public interface ICarouselItem : ISitecoreItem
    {
        #region Properties

        ImageField Image { get; set; }

        string Content { get; set; }

        #endregion
    }
}