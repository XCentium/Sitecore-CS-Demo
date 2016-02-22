using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceStaticCatalogGroup : SitecoreItem, ISitecoreItem, ICommerceStaticCatalogGroup {
        
        #region Members
        public const string SitecoreItemTemplateId = "{71C6AEE2-092E-47CE-9A6F-1A8B91BE4CA2}";
        
        public const string DescriptionFieldId = "{17F206ED-D928-41E8-94D8-F32C4D6B0909}";
        
        public const string DescriptionFieldName = "Description";
        
        public const string IncludedCatalogsFieldId = "{2944FCEE-D46D-4287-81AA-3CD6D979B467}";
        
        public const string IncludedCatalogsFieldName = "IncludedCatalogs";
        #endregion
        
        #region Properties
[SitecoreItemField(DescriptionFieldId)] 
 public virtual string Description { get; set; } 

[SitecoreItemField(IncludedCatalogsFieldId)] 
 public virtual string IncludedCatalogs { get; set; } 
#endregion
        
    }
}
