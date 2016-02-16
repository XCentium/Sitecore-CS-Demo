using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceSettingItem : SitecoreItem, ISitecoreItem, ICommerceSettingItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{774E6D37-F59E-432D-BFE5-89A25B50ABCB}";
        #endregion
    }
}
