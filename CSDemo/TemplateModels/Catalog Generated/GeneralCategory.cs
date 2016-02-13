using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class GeneralCategory : SitecoreItem, ISitecoreItem, IGeneralCategory {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C118EAAE-D723-4560-ABFC-917E58F46F18}";
        
        public const string BrandFieldId = "{913A5D7B-E58A-4882-8630-6BFF128BFCD3}";
        
        public const string BrandFieldName = "Brand";
        
        public const string DescriptionFieldId = "{EB81823C-790D-4072-BAAF-8CC3E41033B6}";
        
        public const string DescriptionFieldName = "Description";
        
        public const string ImagesFieldId = "{7F8D099C-F00C-4F55-A752-CA06116121CD}";
        
        public const string ImagesFieldName = "Images";
        #endregion
        
        #region Properties
[SitecoreItemField(BrandFieldId)] 
 public virtual string Brand { get; set; } 

[SitecoreItemField(DescriptionFieldId)] 
 public virtual string Description { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 
#endregion
        
    }
}
