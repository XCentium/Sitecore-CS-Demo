using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Navigation : SitecoreItem, ISitecoreItem, INavigation {
        
        #region Members
        public const string SitecoreItemTemplateId = "{06E5E610-6DC4-4942-AC32-FCFA2735DE60}";
        
        public const string NameFieldId = "{44B5D5A8-1DBD-49E8-B060-B046EE5430A8}";
        
        public const string NameFieldName = "Name";
        #endregion
        
        #region Properties
[SitecoreItemField(NameFieldId)] 
 public virtual string Name { get; set; } 
#endregion
        
    }
}