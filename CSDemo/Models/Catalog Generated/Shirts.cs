using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Shirts : SitecoreItem, ISitecoreItem, IShirts {
        
        #region Members
        public const string SitecoreItemTemplateId = "{109EE7C1-8D5E-485D-8FC7-698B7934A3BF}";
        
        public const string ImagesFieldId = "{CA7B9E92-6176-4591-B81F-1D3B7C66C9FF}";
        
        public const string ImagesFieldName = "Images";
        
        public const string BrandFieldId = "{072116CD-FD3F-4C33-9316-66539C4C6D1D}";
        
        public const string BrandFieldName = "Brand";
        
        public const string IntroductionDateFieldId = "{EF7C5582-DDDB-473B-BB39-AA4FC523E026}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductIdFieldId = "{F45CA240-D377-4B5A-814B-ACE891D2B9F4}";
        
        public const string ProductIdFieldName = "ProductId";
        #endregion
        
        #region Properties
[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(BrandFieldId)] 
 public virtual string Brand { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 
#endregion
        
    }
}
