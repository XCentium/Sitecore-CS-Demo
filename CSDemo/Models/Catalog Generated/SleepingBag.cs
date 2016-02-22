using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class SleepingBag : SitecoreItem, ISitecoreItem, ISleepingBag {
        
        #region Members
        public const string SitecoreItemTemplateId = "{128D619E-EF28-4CA5-93AB-8AE4D550DB3A}";
        
        public const string IntroductionDateFieldId = "{FA9BC155-A580-400F-97AE-2B80653B2156}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductIdFieldId = "{919C0437-A31D-4C29-9AA4-F4C2CF80DEEF}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{D1CF5745-44B4-478E-89CE-BE6802040EBC}";
        
        public const string ImagesFieldName = "Images";
        #endregion
        
        #region Properties
[SitecoreItemField(IntroductionDateFieldId)] 
 public virtual DateTime IntroductionDate { get; set; } 

[SitecoreItemField(ProductIdFieldId)] 
 public virtual string ProductId { get; set; } 

[SitecoreItemField(ImagesFieldId)] 
 public virtual IEnumerable<Item> Images { get; set; } 
#endregion
        
    }
}
