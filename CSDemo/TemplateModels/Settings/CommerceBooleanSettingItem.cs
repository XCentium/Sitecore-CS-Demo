using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceBooleanSettingItem : SitecoreItem, ISitecoreItem, ICommerceBooleanSettingItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{EC3A4A56-12D5-4883-B809-501F85043CED}";
        
        public const string ValueFieldId = "{4F17697F-BD00-4D76-B4F5-1B53C0C8D884}";
        
        public const string ValueFieldName = "Value";
        #endregion
        
        #region Properties
[SitecoreItemField(ValueFieldId)] 
 public virtual bool Value { get; set; } 
#endregion
        
    }
}
