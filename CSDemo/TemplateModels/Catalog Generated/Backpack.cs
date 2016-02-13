using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Backpack : SitecoreItem, ISitecoreItem, IBackpack {
        
        #region Members
        public const string SitecoreItemTemplateId = "{E89C9114-0705-4064-B992-DD10B009FE15}";
        
        public const string ImagesFieldId = "{50C4FA0C-9740-4BC1-9C1D-FA9AF1A0E19A}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{00120767-ABA0-476C-A929-E80FEF253671}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductIdFieldId = "{24996A9D-4CE8-4C38-87A1-BD91037E473E}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string BrandFieldId = "{6EA7B7F6-DB06-4AF8-96EE-E55868F911F0}";
        
        public const string BrandFieldName = "Brand";
        #endregion
        
        #region Properties
[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(BrandFieldId)] 
 public virtual string Brand { get; set; } 
#endregion
        
    }
}
