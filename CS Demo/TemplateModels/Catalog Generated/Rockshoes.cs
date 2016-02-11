using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Rockshoes : SitecoreItem, ISitecoreItem, IRockshoes {
        
        #region Members
        public const string SitecoreItemTemplateId = "{A5F26A78-7D2E-4603-AC2D-D1CD185E8BF2}";
        
        public const string IntroductionDateFieldId = "{BCB54869-8ACF-4E34-BBD4-6FEAD73BDA7C}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ImagesFieldId = "{583AF565-3663-4942-9C58-BD78B7A7C7CC}";
        
        public const string ImagesFieldName = "Images";
        
        public const string ProductSizeFieldId = "{F3863098-0954-4ED9-A62E-651C7F7993FA}";
        
        public const string ProductSizeFieldName = "ProductSize";
        
        public const string ProductIdFieldId = "{AC8C603C-76B0-40D5-B0B0-9009F3EB4C09}";
        
        public const string ProductIdFieldName = "ProductId";
        #endregion
        
        #region Properties
[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 
#endregion
        
    }
}
