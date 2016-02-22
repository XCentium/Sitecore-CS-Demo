using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalogSettingItem : SitecoreItem, ISitecoreItem, ICommerceCatalogSettingItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{F8C2D048-1934-4EB1-9C5F-8C67DB673A30}";
        
        public const string CatalogFieldId = "{8BBC9900-067F-40EA-8500-8B2C247636F7}";
        
        public const string CatalogFieldName = "Catalog";
        #endregion
        
        #region Properties
[SitecoreItemField(CatalogFieldId)] 
 public virtual string Catalog { get; set; } 
#endregion
        
    }
}
