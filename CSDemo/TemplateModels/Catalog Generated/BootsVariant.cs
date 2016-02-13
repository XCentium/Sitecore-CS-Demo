using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class BootsVariant : SitecoreItem, ISitecoreItem, IBootsVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{D3CF19B1-1D89-4AF0-9F96-617C111346C5}";
        
        public const string VariantIdFieldId = "{F663447F-0689-4168-B9FB-54478861B4F1}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string VariantImagesFieldId = "{EB8DA235-1E8E-4750-826C-0A3EBAA20D9C}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string ProductColorFieldId = "{243BC2AE-346A-4BEB-8164-7FF30F8F759C}";
        
        public const string ProductColorFieldName = "ProductColor";
        #endregion
        
        #region Properties
[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 

[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 
#endregion
        
    }
}
