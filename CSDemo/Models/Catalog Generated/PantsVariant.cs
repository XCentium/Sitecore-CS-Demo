using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class PantsVariant : SitecoreItem, ISitecoreItem, IPantsVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{1FF280D5-6C75-4E88-86CB-4BCE929F1BF2}";
        
        public const string VariantImagesFieldId = "{AC0EABC4-8701-48E3-B72E-E4B070A87335}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string VariantIdFieldId = "{49D2CF8C-E5E9-4843-99EB-D2B775315759}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string ProductColorFieldId = "{49673099-7F65-4679-B209-5916F1C0403D}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string ProductSizeFieldId = "{6CE250FA-2FC9-49CF-BFB7-FE8E4FBF3789}";
        
        public const string ProductSizeFieldName = "ProductSize";
        #endregion
        
        #region Properties
[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 

[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 
#endregion
        
    }
}
