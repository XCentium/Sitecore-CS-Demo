#region

using System.Collections.Generic;
using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;

#endregion

namespace CSDemo.Models.CatalogGenerated
{
    public partial interface IGeneralCategory : ISitecoreItem
    {
        #region Properties

        string Brand { get; set; }

        string Description { get; set; }

        IEnumerable<Item> Images { get; set; }

        #endregion
    }
}