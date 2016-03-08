#region

using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Contracts.Product
{
    public interface IProduct
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string ProductId { get; set; }
        DateTime DateOfIntroduction { get; set; }
        string FullDescription { get; set; }
        string Size { get; set; }
        IEnumerable<Image> Images { get; set; }
        string CatalogName { get; set; }
        decimal Price { get; set; }
        string DefinitionName { get; set; }
        string Description { get; set; }
        bool IsOnSale { get; set; }
        string Url { get; set; }
        string CurrencyPrice { get; }
        bool IsNew { get; set; }
        IEnumerable<Item> Categories { get; set; }
        decimal Rating { get; set; }
        string ItemsPerPage { get; set; }
        string Variants { get; set; }
        string Brand { get; set; }
        IEnumerable<Item> SortFields { get; set; }
    }
}