using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalogGroupFolder : SitecoreItem, ISitecoreItem, ICommerceCatalogGroupFolder {
        
        #region Members
        public const string SitecoreItemTemplateId = "{857F5B7E-B382-453C-B6D8-E0C2E65C7160}";
        #endregion
    }
}
