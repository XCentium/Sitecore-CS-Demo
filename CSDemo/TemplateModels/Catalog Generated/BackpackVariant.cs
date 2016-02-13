using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class BackpackVariant : SitecoreItem, ISitecoreItem, IBackpackVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{5A6BE34E-6E7A-4498-B6B6-613879C3BE84}";
        
        public const string ProductColorFieldId = "{67AA1666-FF2F-4381-8CB5-A60D985F059D}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string VariantImagesFieldId = "{B3AA65A8-F216-44E6-A716-07A952ECD89F}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string VariantIdFieldId = "{D4C5251A-58D9-45F6-A1F1-E53E241D6677}";
        
        public const string VariantIdFieldName = "VariantId";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 

[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 
#endregion
        
    }
}
