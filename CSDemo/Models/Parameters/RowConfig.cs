using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class RowConfig : SitecoreItem, ISitecoreItem, IRowConfig {
        
        #region Members
        public const string SitecoreItemTemplateId = "{7B654478-F079-4A49-A93A-840E17FE99E8}";
        
        public const string SectionCSSClassFieldId = "{A9870CE6-6898-406D-BC6A-5C29395D2797}";
        
        public const string SectionCSSClassFieldName = "Section CSS Class";
        
        public const string ContainerCSSClassFieldId = "{7E0A5E69-D685-4FD0-AE8D-9130188DE285}";
        
        public const string ContainerCSSClassFieldName = "Container CSS Class";
        
        public const string RowCSSClassFieldId = "{48216F20-9ED5-4225-9CEA-584C66666F2C}";
        
        public const string RowCSSClassFieldName = "Row CSS Class";
        #endregion
        
        #region Properties
[SitecoreItemField(SectionCSSClassFieldId)] 
 public virtual string SectionCSSClass { get; set; } 

[SitecoreItemField(ContainerCSSClassFieldId)] 
 public virtual string ContainerCSSClass { get; set; } 

[SitecoreItemField(RowCSSClassFieldId)] 
 public virtual string RowCSSClass { get; set; } 
#endregion
        
    }
}
