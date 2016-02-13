using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Supplies : SitecoreItem, ISitecoreItem, ISupplies {
        
        #region Members
        public const string SitecoreItemTemplateId = "{34FD3D83-F412-4BA8-B1FE-66D253FC0D8E}";
        
        public const string ProductIdFieldId = "{2E4F81AD-589D-4312-A828-16506FA867A9}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{F2DC27E5-6E93-4131-ACC8-94EC09BDF075}";
        
        public const string ImagesFieldName = "Images";
        
        public const string IntroductionDateFieldId = "{49B0E774-9ED8-4790-BC53-4A33D728C879}";
        
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
