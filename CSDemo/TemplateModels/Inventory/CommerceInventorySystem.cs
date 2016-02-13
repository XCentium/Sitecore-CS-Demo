using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Inventory {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceInventorySystem : SitecoreItem, ISitecoreItem, ICommerceInventorySystem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{4288E630-621F-4107-9B1F-413D446ACF75}";
        #endregion
    }
}
