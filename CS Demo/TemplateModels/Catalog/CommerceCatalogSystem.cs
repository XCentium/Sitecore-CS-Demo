using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalogSystem : SitecoreItem, ISitecoreItem, ICommerceCatalogSystem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C8B59B26-8D1F-46BB-BCCE-7ED5945BF3CF}";
        #endregion
    }
}
