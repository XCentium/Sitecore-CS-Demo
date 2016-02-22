using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class TentsVariant : SitecoreItem, ISitecoreItem, ITentsVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{E985B15A-C144-473C-A769-8A9EFD4CD88E}";
        
        public const string ProductColorFieldId = "{9AE73DE1-AF53-4289-9C0A-04482182621E}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string VariantImagesFieldId = "{AB55DC2D-8403-430D-8071-3788777B5AD4}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        
        public const string VariantIdFieldId = "{EB4D34F7-AE69-4101-8202-CA4683E55651}";
        
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
