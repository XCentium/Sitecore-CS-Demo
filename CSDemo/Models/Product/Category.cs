#region

using System;
using System.Collections.Generic;
using System.Globalization;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using CSDemo.Models.Product;

#endregion

namespace CSDemo.Models
{


    [SitecoreType(AutoMap = true)]
    public partial class Category : ICategory
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }
        [SitecoreField(Fields.CatalogName)]
        public virtual string CatalogName { get; set; }
        [SitecoreField(Fields.Brand)]
        public virtual string Brand { get; set; }
        [SitecoreField(Fields.ChildProducts)]
        public IEnumerable<Item> ChildProducts { get; set; }
        [SitecoreField(Fields.RuntimeSearchFacets)]
        public IEnumerable<Item> RuntimeSearchFacets { get; set; }
        [SitecoreField(Fields.ChildCategories)]
        public IEnumerable<Item> ChildCategories { get; set; }
        [SitecoreField(Fields.RelationshipList)]
        public virtual string RelationshipList { get; set; }
        [SitecoreField(Fields.DefinitionName)]
        public virtual string DefinitionName { get; set; }
        [SitecoreField(Fields.ToolsIcon)]
        public virtual string ToolsIcon { get; set; }
        [SitecoreField(Fields.PrimaryParentCategory)]
        public IEnumerable<Item> PrimaryParentCategory { get; set; }
        [SitecoreField(Fields.SortFields)]
        public IEnumerable<Item> SortFields { get; set; }
        [SitecoreField(Fields.ItemsPerPage)]
        public virtual string ItemsPerPage { get; set; }
        [SitecoreField(Fields.ToolsSearchFacets)]
        public IEnumerable<Item> ToolsSearchFacets { get; set; }
        [SitecoreField(Fields.ToolsNavigationFacets)]
        public IEnumerable<Item> ToolsNavigationFacets { get; set; }
        [SitecoreField(Fields.ParentCategories)]
        public virtual string ParentCategories { get; set; }
        [SitecoreField(Fields.Description)]
        public virtual string Description { get; set; }
        [SitecoreField(Fields.ListPrice)]
        public virtual string ListPrice { get; set; }

        [SitecoreField(Fields.Images)]
        public IEnumerable<Item> Images { get; set; }

        public IEnumerable<CSDemo.Models.Product.Product> Products { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string CatalogName = "CatalogName";
            public const string Brand = "Brand";
            public const string ChildProducts = "ChildProducts";
            public const string RuntimeSearchFacets = "Runtime Search Facets";
            public const string ChildCategories = "ChildCategories";
            public const string RelationshipList = "Relationship List";
            public const string DefinitionName = "DefinitionName";
            public const string ToolsIcon = "ToolsIcon";
            public const string PrimaryParentCategory = "PrimaryParentCategory";
            public const string SortFields = "Sort Fields";
            public const string ItemsPerPage = "Items Per Page";
            public const string ToolsSearchFacets = "Tools Search Facets";
            public const string ToolsNavigationFacets = "Tools Navigation Facets";
            public const string ParentCategories = "ParentCategories";
            public const string Description = "Description";
            public const string ListPrice = "ListPrice";
            public const string Images = "Images";
        }
        #endregion 


    }
}