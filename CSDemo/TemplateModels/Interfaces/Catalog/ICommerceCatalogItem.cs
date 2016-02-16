using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceCatalogItem : ISitecoreItem {
        
        #region Properties
string RelationshipList { get; set; } 

string PrimaryParentCategory { get; set; } 

string ParentCategories { get; set; } 

string CatalogName { get; set; } 

string ListPrice { get; set; } 

string DefinitionName { get; set; } 
#endregion
        
    }
}
