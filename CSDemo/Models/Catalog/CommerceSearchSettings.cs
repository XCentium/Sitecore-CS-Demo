using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceSearchSettings : SitecoreItem, ISitecoreItem, ICommerceSearchSettings {
        
        #region Members
        public const string SitecoreItemTemplateId = "{CB5F3E43-EAF7-4EDB-8235-674745D95059}";
        
        public const string ToolsSearchFacetsFieldId = "{420373A7-F888-490A-ABD6-E1BA4FCB75E0}";
        
        public const string ToolsSearchFacetsFieldName = "Tools Search Facets";
        
        public const string SortFieldsFieldId = "{05124B76-FA67-4123-87B9-16E1883E9244}";
        
        public const string SortFieldsFieldName = "Sort Fields";
        
        public const string ToolsNavigationFacetsFieldId = "{B5E7B1DF-4155-4862-8D81-1E63ED6D3BC2}";
        
        public const string ToolsNavigationFacetsFieldName = "Tools Navigation Facets";
        
        public const string RuntimeSearchFacetsFieldId = "{7E48EE19-93FC-4071-A192-470F1259EAE0}";
        
        public const string RuntimeSearchFacetsFieldName = "Runtime Search Facets";
        
        public const string ItemsPerPageFieldId = "{08B14202-D89B-42A5-A7AD-CD1F389DDB07}";
        
        public const string ItemsPerPageFieldName = "Items Per Page";
        #endregion
        
        #region Properties
[SitecoreItemField(ToolsSearchFacetsFieldId)] 
 public virtual IEnumerable<Item> ToolsSearchFacets { get; set; } 

[SitecoreItemField(SortFieldsFieldId)] 
 public virtual IEnumerable<Item> SortFields { get; set; } 

[SitecoreItemField(ToolsNavigationFacetsFieldId)] 
 public virtual IEnumerable<Item> ToolsNavigationFacets { get; set; } 

[SitecoreItemField(RuntimeSearchFacetsFieldId)] 
 public virtual IEnumerable<Item> RuntimeSearchFacets { get; set; } 

[SitecoreItemField(ItemsPerPageFieldId)] 
 public virtual string ItemsPerPage { get; set; } 
#endregion
        
    }
}
