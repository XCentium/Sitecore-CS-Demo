using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceSettingsFolder : SitecoreItem, ISitecoreItem, ICommerceSettingsFolder {
        
        #region Members
        public const string SitecoreItemTemplateId = "{FABAA220-6FD2-4138-B307-752F6DA5BA30}";
        #endregion
    }
}
