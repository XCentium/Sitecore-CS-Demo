using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceDatetimeSettingItem : SitecoreItem, ISitecoreItem, ICommerceDatetimeSettingItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{ED7EF43B-D2D4-4F5D-A8A2-AFEEB2F611D0}";
        
        public const string ValueFieldId = "{D1B895F9-8487-4308-8FCB-F73F45BB7CD9}";
        
        public const string ValueFieldName = "Value";
        #endregion
        
        #region Properties
[SitecoreItemField(ValueFieldId)] 
 public virtual DateTime Value { get; set; } 
#endregion
        
    }
}
