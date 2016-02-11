using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Settings {
    
    
    public partial interface ICommerceDatetimeSettingItem : ISitecoreItem {
        
        #region Properties
DateTime Value { get; set; } 
#endregion
        
    }
}
