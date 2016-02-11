using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceSearchSettings : ISitecoreItem {
        
        #region Properties
IEnumerable<Item> ToolsSearchFacets { get; set; } 

IEnumerable<Item> SortFields { get; set; } 

IEnumerable<Item> ToolsNavigationFacets { get; set; } 

IEnumerable<Item> RuntimeSearchFacets { get; set; } 

string ItemsPerPage { get; set; } 
#endregion
        
    }
}
