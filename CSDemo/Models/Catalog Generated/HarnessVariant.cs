using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class HarnessVariant : SitecoreItem, ISitecoreItem, IHarnessVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{6BE34CAA-F102-4384-93B7-046A86E2BC47}";
        
        public const string VariantImagesFieldId = "{68717157-A5B5-4937-B070-50D67BA73C5B}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string VariantIdFieldId = "{4453F26E-50C8-4414-92D3-A97BABC15EA7}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string ProductColorFieldId = "{64F1E324-BF75-4584-BEAF-32D09F894975}";
        
        public const string ProductColorFieldName = "ProductColor";
        #endregion
        
        #region Properties
[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 
#endregion
        
    }
}
