using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class ShirtsVariant : SitecoreItem, ISitecoreItem, IShirtsVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C7F9C209-D806-4AF3-B1CC-F2B4454987FF}";
        
        public const string ProductSizeFieldId = "{FE8CCF30-0557-45AD-86EB-EE9A90F88B4E}";
        
        public const string ProductSizeFieldName = "ProductSize";
        
        public const string VariantImagesFieldId = "{DE8FEABA-FE3E-40E7-BD4A-EE5A93321829}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string ProductColorFieldId = "{31B0A7AA-E30A-4FCB-AA67-164172A47A7B}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string VariantIdFieldId = "{71D8B628-2D8F-48F4-8692-194ED77B93DD}";
        
        public const string VariantIdFieldName = "VariantId";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 

[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 

[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 
#endregion
        
    }
}
