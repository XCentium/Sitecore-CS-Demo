#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;

#endregion

namespace CSDemo.Contracts.Navigation
{
    public partial interface INavigationLink : ISitecoreItem
    {
        #region Properties

        LinkField Link { get; set; }

        Item MegaLink { get; set; }

        #endregion
    }
}