using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    public partial interface ICategorylistingConfig : ISitecoreItem {
        
        #region Properties
Item TargetCatalogue { get; set; } 
#endregion
        
    }
}
