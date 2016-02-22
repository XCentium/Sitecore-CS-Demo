using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Pages {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class HomePage : SitecoreItem, ISitecoreItem, IHomePage {
        
        #region Members
        public const string SitecoreItemTemplateId = "{27707EF4-EA14-439A-BA20-DBD16605AE04}";
        #endregion
    }
}
