#region

using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper;

#endregion

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