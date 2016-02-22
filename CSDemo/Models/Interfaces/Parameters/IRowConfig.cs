using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    public partial interface IRowConfig : ISitecoreItem {
        
        #region Properties
string SectionCSSClass { get; set; } 

string ContainerCSSClass { get; set; } 

string RowCSSClass { get; set; } 
#endregion
        
    }
}
