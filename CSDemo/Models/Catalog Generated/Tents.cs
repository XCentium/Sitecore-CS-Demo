using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Tents : SitecoreItem, ISitecoreItem, ITents {
        
        #region Members
        public const string SitecoreItemTemplateId = "{F53DDD83-3D71-4501-8D30-2EEAE92CC0D1}";
        
        public const string IntroductionDateFieldId = "{73D3E7CD-FB9E-4FE7-83A7-7E9A5658C669}";
        
        public const string IntroductionDateFieldName = "IntroductionDate";
        
        public const string ProductIdFieldId = "{C5F04A3A-2319-4888-8A9F-700FF7064A08}";
        
        public const string ProductIdFieldName = "ProductId";
        
        public const string ImagesFieldId = "{6A9662AF-B1B2-4DB0-8DF7-58F0BF2B25F4}";
        
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
