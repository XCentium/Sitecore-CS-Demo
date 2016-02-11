using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Boots : SitecoreItem, ISitecoreItem, IBoots {
        
        #region Members
        public const string SitecoreItemTemplateId = "{6C29C67A-72F1-4AEE-900E-A10EFB3FBC01}";
        
        public const string ProductIdFieldId = "{845CFB9B-80AA-40BF-A965-4060DD5FD53F}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{65AF1443-9A4B-4371-B485-CA6204E26D29}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{BF8546AE-40EB-4B0F-AD50-57C89BE776E3}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string BrandFieldId = "{060BD307-FCDE-4D66-9097-37DE102D8058}";
        
        public const string BrandFieldName = "Brand";
        
        public const string ProductSizeFieldId = "{CACF7722-3E6A-4CFC-AF64-0063B8F1BE3A}";
        
        public const string ProductSizeFieldName = "ProductSize";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(BrandFieldId)] 
 public virtual string Brand { get; set; } 

[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 
#endregion
        
    }
}
