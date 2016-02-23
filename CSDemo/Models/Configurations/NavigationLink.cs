using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Configurations {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class NavigationLink : SitecoreItem, ISitecoreItem, INavigationLink {
        
        #region Members
        public const string SitecoreItemTemplateId = "{99549356-0172-4D02-82C3-8EDEF477DA33}";
        
        public const string MegaLinkFieldId = "{E60061C8-BCB6-4126-8904-1C5E7E2F5B0A}";
        
        public const string MegaLinkFieldName = "Mega Link";
        
        public const string NameFieldId = "{52C77500-7897-4BB2-A232-D6520FE849EC}";
        
        public const string NameFieldName = "Name";
        
        public const string LinkFieldId = "{63C29299-7A35-4A1E-BA9E-2E1C41B07DDA}";
        
        public const string LinkFieldName = "Link";
        #endregion
        
        #region Properties
[SitecoreItemField(MegaLinkFieldId)] 
 public virtual Item MegaLink { get; set; } 

[SitecoreItemField(NameFieldId)] 
 public virtual string Name { get; set; } 

[SitecoreItemField(LinkFieldId)] 
 public virtual LinkField Link { get; set; } 
#endregion
        
    }
}
