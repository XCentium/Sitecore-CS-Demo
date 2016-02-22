using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Column2Config : SitecoreItem, ISitecoreItem, IColumn2Config {
        
        #region Members
        public const string SitecoreItemTemplateId = "{18D71C58-1DF4-4F15-B36C-F7065D33650C}";
        
        public const string Column2CSSClassFieldId = "{6760DCCF-F40F-403A-B39C-734D08F10326}";
        
        public const string Column2CSSClassFieldName = "Column2 CSS Class";
        #endregion
        
        #region Properties
[SitecoreItemField(Column2CSSClassFieldId)] 
 public virtual string Column2CSSClass { get; set; } 
#endregion
        
    }
}
