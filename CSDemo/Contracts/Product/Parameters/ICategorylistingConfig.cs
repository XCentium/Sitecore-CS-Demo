#region

using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;

#endregion

namespace CSDemo.Models.Parameters
{
    public partial interface ICategorylistingConfig : ISitecoreItem
    {
        #region Properties

        Item TargetCatalogue { get; set; }

        #endregion
    }
}