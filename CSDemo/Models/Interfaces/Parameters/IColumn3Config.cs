using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    public partial interface IColumn3Config : ISitecoreItem {
        
        #region Properties
string Column3CSSClass { get; set; } 
#endregion
        
    }
}
