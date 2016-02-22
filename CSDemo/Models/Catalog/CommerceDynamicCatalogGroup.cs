using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceDynamicCatalogGroup : SitecoreItem, ISitecoreItem, ICommerceDynamicCatalogGroup {
        
        #region Members
        public const string SitecoreItemTemplateId = "{23AA0570-78BB-40BB-8DCF-DEF77500C669}";
        
        public const string DescriptionFieldId = "{FFC8DA72-A86D-488F-B7CD-B9CCB0C93B6D}";
        
        public const string DescriptionFieldName = "Description";
        
        public const string ExpressionFieldId = "{56937DFB-E75B-46A6-87C9-0F5BEC5AFBD1}";
        
        public const string ExpressionFieldName = "Expression";
        
        public const string IncludedCatalogsFieldId = "{6F3AE757-920F-4519-8C44-3580DB45F129}";
        
        public const string IncludedCatalogsFieldName = "IncludedCatalogs";
        #endregion
        
        #region Properties
[SitecoreItemField(DescriptionFieldId)] 
 public virtual string Description { get; set; } 

[SitecoreItemField(ExpressionFieldId)] 
 public virtual string Expression { get; set; } 

[SitecoreItemField(IncludedCatalogsFieldId)] 
 public virtual string IncludedCatalogs { get; set; } 
#endregion
        
    }
}
