using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Crampon : SitecoreItem, ISitecoreItem, ICrampon {
        
        #region Members
        public const string SitecoreItemTemplateId = "{B27E34CC-A6C1-4E3E-952D-0B613A17170B}";
        
        public const string ProductSizeFieldId = "{78145B6E-728E-4D12-9FB2-F5A080B27C1D}";
        
        public const string ProductSizeFieldName = "ProductSize";
        
        public const string ProductIdFieldId = "{2DEF2F50-FE2D-4169-A5EF-A0D35A4192F7}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{5D578630-6315-41B0-97B9-22646F577066}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{519A4DF3-8FA8-4DF9-88C0-54FADB4B0F9C}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 
#endregion
        
    }
}
