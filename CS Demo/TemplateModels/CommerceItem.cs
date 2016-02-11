using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceItem : SitecoreItem, ISitecoreItem, ICommerceItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{AC480A37-481D-4BB3-AEC5-D35FB363835F}";
        
        public const string ToolsIconFieldId = "{738B32EE-B885-46A2-97C7-645915EBA217}";
        
        public const string ToolsIconFieldName = "ToolsIcon";
        #endregion
        
        #region Properties
[SitecoreItemField(ToolsIconFieldId)] 
 public virtual string ToolsIcon { get; set; } 
#endregion
        
    }
}
