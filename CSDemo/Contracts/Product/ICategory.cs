#region

using System.Collections.Generic;
using Sitecore.Data.Items;
using Glass.Mapper.Sc.Fields;

#endregion

namespace CSDemo.Contracts.Product
{
    public partial interface ICategory
    {

        #region Properties
        string CatalogName { get; set; }
        string Brand { get; set; }
        IEnumerable<Item> ChildProducts { get; set; }
        IEnumerable<Item> RuntimeSearchFacets { get; set; }
        IEnumerable<Item> ChildCategories { get; set; }
        string RelationshipList { get; set; }
        string DefinitionName { get; set; }
        string ToolsIcon { get; set; }
        IEnumerable<Item> PrimaryParentCategory { get; set; }
        IEnumerable<Item> SortFields { get; set; }
        string ItemsPerPage { get; set; }
        IEnumerable<Item> ToolsSearchFacets { get; set; }
        IEnumerable<Item> ToolsNavigationFacets { get; set; }
        string ParentCategories { get; set; }
        string Description { get; set; }
        string ListPrice { get; set; }
        IEnumerable<Image> Images { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion 



    }
}
