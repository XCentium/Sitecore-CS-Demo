using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Pants : SitecoreItem, ISitecoreItem, IPants {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C9D7303D-F695-481E-A2C8-FB397013C967}";
        
        public const string BrandFieldId = "{7B243DF0-4C10-4395-A87C-CC4D97270286}";
        
        public const string BrandFieldName = "Brand";
        
        public const string IntroductionDateFieldId = "{A173337F-1A62-4093-986E-68F164F3CC9C}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ImagesFieldId = "{F553FE05-02AB-4124-A49D-0AA48E426E3F}";
        
        public const string ImagesFieldName = "Images";
        
        public const string ProductIdFieldId = "{AE6BDF17-9527-4ED7-A364-A59F724764D6}";
        
        public const string ProductIdFieldName = "ProductId";
        #endregion
        
        #region Properties
[SitecoreItemField(BrandFieldId)] 
 public virtual string Brand { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 
#endregion
        
    }
}
