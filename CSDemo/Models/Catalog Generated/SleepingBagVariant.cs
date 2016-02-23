using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class SleepingBagVariant : SitecoreItem, ISitecoreItem, ISleepingBagVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{3394D994-85BA-46F8-AB89-3913367E6A26}";
        
        public const string VariantIdFieldId = "{1325F917-0B86-4E88-B5CA-BD6D052EC85A}";
        
        public const string VariantIdFieldName = "VariantId";
        
        public const string ProductColorFieldId = "{52EB3E6A-381A-4929-A72D-CB8F421C7B7E}";
        
        public const string ProductColorFieldName = "ProductColor";
        
        public const string VariantImagesFieldId = "{5735209F-7B98-4563-A032-36F767AC7960}";
        
        public const string VariantImagesFieldName = "Variant_Images";
        #endregion
        
        #region Properties
[SitecoreItemField(VariantIdFieldId)] 
 public virtual string VariantId { get; set; } 

[SitecoreItemField(ProductColorFieldId)] 
 public virtual string ProductColor { get; set; } 

[SitecoreItemField(VariantImagesFieldId)] 
 public virtual IEnumerable<Item> VariantImages { get; set; } 
#endregion
        
    }
}
