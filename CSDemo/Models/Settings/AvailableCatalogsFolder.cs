using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class AvailableCatalogsFolder : SitecoreItem, ISitecoreItem, IAvailableCatalogsFolder {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C9983EE2-A307-4D4E-8AE6-FE269A7A2F73}";
        #endregion
    }
}
