using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Carabiner : SitecoreItem, ISitecoreItem, ICarabiner {
        
        #region Members
        public const string SitecoreItemTemplateId = "{894C91FA-1038-47F6-ACF7-1DE2677640E9}";
        
        public const string ProductIdFieldId = "{3870F8CB-4F15-45DC-8075-7DC0FBAE730F}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{B97A7E02-7816-4616-B25A-5546E62A4E2C}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{F72651E7-D37B-42DE-8116-7FA66B7AAADF}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        #endregion
        
        #region Properties
[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 

[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 
#endregion
        
    }
}
