using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Inventory {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class InventoryCatalog : SitecoreItem, ISitecoreItem, IInventoryCatalog {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C5E8C469-5580-4755-93C5-F62E11D014A5}";
        
        public const string InventoryCatalogNameFieldId = "{E76788A0-7284-4628-9A14-6F9B04061EC9}";
        
        public const string InventoryCatalogNameFieldName = "InventoryCatalogName";
        
        public const string DateCreatedFieldId = "{87BAC990-23F1-4AFB-87C0-63AA0AB81DB2}";
        
        public const string DateCreatedFieldName = "DateCreated";
        
        public const string InventoryCatalogDescriptionFieldId = "{58128D95-FA81-4DDB-85D6-099ACB73BF3D}";
        
        public const string InventoryCatalogDescriptionFieldName = "InventoryCatalogDescription";
        
        public const string LastModifiedFieldId = "{731CB645-FAA3-4440-9511-A27556A63AD9}";
        
        public const string LastModifiedFieldName = "LastModified";
        #endregion
        
        #region Properties
[SitecoreItemField(InventoryCatalogNameFieldId)] 
 public virtual string InventoryCatalogName { get; set; } 

[SitecoreItemField(DateCreatedFieldId)] 
 public virtual string DateCreated { get; set; } 

[SitecoreItemField(InventoryCatalogDescriptionFieldId)] 
 public virtual string InventoryCatalogDescription { get; set; } 

[SitecoreItemField(LastModifiedFieldId)] 
 public virtual string LastModified { get; set; } 
#endregion
        
    }
}
