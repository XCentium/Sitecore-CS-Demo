using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class ParkaVariant : SitecoreItem, ISitecoreItem, IParkaVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{07AB50ED-DF81-4D82-95EC-28E8095E087D}";
        
        public const string ProductColorFieldId = "{A1FE6409-1E00-4987-B0D4-F175CA0A8C1A}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string VariantIdFieldId = "{05738457-1883-4CE2-84EE-9BF90E999D28}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string VariantImagesFieldId = "{61ECA7E7-DD0D-4CD3-9973-BDD4B6A2B5F2}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 

[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 
#endregion
        
    }
}
