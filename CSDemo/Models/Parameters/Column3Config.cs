using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Column3Config : SitecoreItem, ISitecoreItem, IColumn3Config {
        
        #region Members
        public const string SitecoreItemTemplateId = "{101CEB4D-E37C-49A0-883E-865EF4CD9216}";
        
        public const string Column3CSSClassFieldId = "{D3E84558-9CC0-438B-A4CE-564BF5A022EC}";
        
        public const string Column3CSSClassFieldName = "Column3 CSS Class";
        #endregion
        
        #region Properties
[SitecoreItemField(Column3CSSClassFieldId)] 
 public virtual string Column3CSSClass { get; set; } 
#endregion
        
    }
}
