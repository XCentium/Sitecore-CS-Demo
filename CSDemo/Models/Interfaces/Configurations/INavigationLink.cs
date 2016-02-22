using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Configurations {
    
    
    public partial interface INavigationLink : ISitecoreItem {
        
        #region Properties
Item MegaLink { get; set; } 

string Name { get; set; } 

LinkField Link { get; set; } 
#endregion
        
    }
}
