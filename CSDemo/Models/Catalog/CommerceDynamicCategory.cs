using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceDynamicCategory : SitecoreItem, ISitecoreItem, ICommerceDynamicCategory {
        
        #region Members
        public const string SitecoreItemTemplateId = "{6820281F-3BB3-41B4-8C93-7771EEA496D0}";
        #endregion
    }
}
