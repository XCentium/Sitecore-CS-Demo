using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Components {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class ContentBlock : SitecoreItem, ISitecoreItem, IContentBlock {
        
        #region Members
        public const string SitecoreItemTemplateId = "{98998559-9BBC-414D-B657-09A527F70C4A}";
        #endregion
    }
}
