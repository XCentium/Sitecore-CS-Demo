using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Harness : SitecoreItem, ISitecoreItem, IHarness {
        
        #region Members
        public const string SitecoreItemTemplateId = "{6A3B3239-6E1F-413F-9EE0-364AFC408001}";
        
        public const string ProductIdFieldId = "{49D16ECB-780B-4977-81B5-20DCA6D55319}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string IntroductionDateFieldId = "{DBAAE64F-257B-4FFE-80E8-E600DA58D5A3}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductSizeFieldId = "{86BACC7D-838A-4EA4-B216-B0AC21984C44}";
        
        public const string ProductSizeFieldName = "ProductSize";
        
        public const string ImagesFieldId = "{B960CF19-2332-4E36-A131-067C77B798E5}";
        
        public const string ImagesFieldName = "Images";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ProductSizeFieldId)] 
 public virtual string ProductSize { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 
#endregion
        
    }
}
