using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Parameters {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CategorylistingConfig : SitecoreItem, ISitecoreItem, ICategorylistingConfig {
        
        #region Members
        public const string SitecoreItemTemplateId = "{12AFFA53-D310-4B45-A82B-5A23990410D3}";
        
        public const string TargetCatalogueFieldId = "{5E332FED-8B3E-44D1-A675-D3F3F5827D54}";
        
        public const string TargetCatalogueFieldName = "Target Catalogue";
        #endregion
        
        #region Properties
[SitecoreItemField(TargetCatalogueFieldId)] 
 public virtual Item TargetCatalogue { get; set; } 
#endregion
        
    }
}
