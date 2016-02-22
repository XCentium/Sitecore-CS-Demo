using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Column4Config : SitecoreItem, ISitecoreItem, IColumn4Config {
        
        #region Members
        public const string SitecoreItemTemplateId = "{2C398B88-885D-476B-B0F4-273D1C44385B}";
        
        public const string Column4CSSClassFieldId = "{03F57D6E-7789-4424-B41C-3C9D1470DC6F}";
        
        public const string Column4CSSClassFieldName = "Column4 CSS Class";
        #endregion
        
        #region Properties
[SitecoreItemField(Column4CSSClassFieldId)] 
 public virtual string Column4CSSClass { get; set; } 
#endregion
        
    }
}
