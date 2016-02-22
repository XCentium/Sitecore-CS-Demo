using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Settings {
    
    
    public partial interface ICommerceStringSettingItem : ISitecoreItem {
        
        #region Properties
string Value { get; set; } 
#endregion
        
    }
}
