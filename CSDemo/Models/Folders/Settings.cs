using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Settings : SitecoreItem, ISitecoreItem, ISettings {
        
        #region Members
        public const string SitecoreItemTemplateId = "{A21B890E-C798-469C-8727-0F2C525787F3}";
        #endregion
    }
}
