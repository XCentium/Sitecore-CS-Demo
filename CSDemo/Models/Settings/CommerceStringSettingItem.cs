using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceStringSettingItem : SitecoreItem, ISitecoreItem, ICommerceStringSettingItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{4CF8D7C4-DAE3-4D41-84AC-8EE42AF19F50}";
        
        public const string ValueFieldId = "{48A49B5C-6DF0-4CAB-AAE7-E9BC0E380B3F}";
        
        public const string ValueFieldName = "Value";
        #endregion
        
        #region Properties
[SitecoreItemField(ValueFieldId)] 
 public virtual string Value { get; set; } 
#endregion
        
    }
}
