using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Column1Config : SitecoreItem, ISitecoreItem, IColumn1Config {
        
        #region Members
        public const string SitecoreItemTemplateId = "{0B336C1E-B0F2-40FE-970F-BDB4810B7BFE}";
        
        public const string Column1CSSClassFieldId = "{ABA1551F-ACF0-4397-9009-AD73FB9A2E57}";
        
        public const string Column1CSSClassFieldName = "Column1 CSS Class";
        #endregion
        
        #region Properties
[SitecoreItemField(Column1CSSClassFieldId)] 
 public virtual string Column1CSSClass { get; set; } 
#endregion
        
    }
}
