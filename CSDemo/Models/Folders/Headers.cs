using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Headers : SitecoreItem, ISitecoreItem, IHeaders {
        
        #region Members
        public const string SitecoreItemTemplateId = "{8B3EEE2A-D606-4813-B1F4-B7159378B65E}";
        #endregion
    }
}
