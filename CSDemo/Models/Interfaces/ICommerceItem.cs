using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models {
    
    
    public partial interface ICommerceItem : ISitecoreItem {
        
        #region Properties
string ToolsIcon { get; set; } 
#endregion
        
    }
}
