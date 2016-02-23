using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Parka : SitecoreItem, ISitecoreItem, IParka {
        
        #region Members
        public const string SitecoreItemTemplateId = "{2130E6BD-C8D8-4EB0-B6AF-6C0D89F4A6BB}";
        
        public const string ProductIdFieldId = "{C91CFA56-F2A5-4770-8F56-A786E39735FF}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{5054EB54-5606-4544-BD6B-89003D0FC6F9}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{46F715D6-F01F-4BE8-9F16-028BE0AE6090}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductSizeFieldId = "{0F8456B6-6B79-4ACD-8DFE-E7F3070F2061}";
        
        public const string ProductSizeFieldName = "ProductSize";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 
#endregion
        
    }
}
