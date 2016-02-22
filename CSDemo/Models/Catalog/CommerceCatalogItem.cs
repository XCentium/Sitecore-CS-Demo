using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalogItem : SitecoreItem, ISitecoreItem, ICommerceCatalogItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{E55C2650-E1B7-47F7-A725-0DD761B57CCF}";
        
        public const string RelationshipListFieldId = "{14DAF8A9-B4EC-46E3-AEB0-B3BA43EEAE11}";
        
        public const string RelationshipListFieldName = "Relationship List";
        
        public const string PrimaryParentCategoryFieldId = "{41D45D18-95E9-45E1-BB21-057F9F0E3BB7}";
        
        public const string PrimaryParentCategoryFieldName = "PrimaryParentCategory";
        
        public const string ParentCategoriesFieldId = "{1ADB7882-F838-4D0B-BF63-96B1CDA442EE}";
        
        public const string ParentCategoriesFieldName = "ParentCategories";
        
        public const string CatalogNameFieldId = "{71B0E6A9-C9D1-4972-BB35-A34BBA8DB631}";
        
        public const string CatalogNameFieldName = "CatalogName";
        
        public const string ListPriceFieldId = "{2B935D80-96A1-4D69-A1B1-3B518EA7659E}";
        
        public const string ListPriceFieldName = "ListPrice";
        
        public const string DefinitionNameFieldId = "{221635A5-DB7E-44BC-976A-8D31DACC6025}";
        
        public const string DefinitionNameFieldName = "DefinitionName";
        #endregion
        
        #region Properties
[SitecoreItemField(RelationshipListFieldId)] 
 public virtual string RelationshipList { get; set; } 

[SitecoreItemField(PrimaryParentCategoryFieldId)] 
 public virtual string PrimaryParentCategory { get; set; } 

[SitecoreItemField(ParentCategoriesFieldId)] 
 public virtual string ParentCategories { get; set; } 

[SitecoreItemField(CatalogNameFieldId)] 
 public virtual string CatalogName { get; set; } 

[SitecoreItemField(ListPriceFieldId)] 
 public virtual string ListPrice { get; set; } 

[SitecoreItemField(DefinitionNameFieldId)] 
 public virtual string DefinitionName { get; set; } 
#endregion
        
    }
}
