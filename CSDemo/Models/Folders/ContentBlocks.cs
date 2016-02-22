using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class ContentBlocks : SitecoreItem, ISitecoreItem, IContentBlocks {
        
        #region Members
        public const string SitecoreItemTemplateId = "{F210CB32-084A-48F3-8DC9-A6692CD84A56}";
        #endregion
    }
}
