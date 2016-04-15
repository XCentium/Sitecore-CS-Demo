#region

using System;
using System.Collections.Generic;
using CSDemo.Models.Product;
using Glass.Mapper.Sc.Fields;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Contracts.Product
{
    public interface IProduct
    {
        IEnumerable<ProductVariant> ProductVariants { get; set; }
        IEnumerable<VariantColor> VariantColors { get; set; }
        IEnumerable<Models.Product.Product> AlsoBoughtProducts { get; }
        IEnumerable<Models.Product.Product> RelatedProducts { get; }
        StockInformation StockInformation { get; set; }
        string CurrencyPrice { get; }
        string ListPrice { get; }
        string VariantProdId { get; }
        string VisitorSignupForStockNotificationEmail { get; set; }
        string LocationName { get; set; }
        Guid ID { get; set; }
        string Path { get; set; }
        string Title { get; set; }
        string ProductId { get; set; }
        DateTime DateOfIntroduction { get; set; }
        string FullDescription { get; set; }
        string MoreInfo { get; set; }
        string Size { get; set; }
        IEnumerable<Image> Images { get; set; }
        string CatalogName { get; set; }
        decimal Price { get; set; }
        decimal SalePrice { get; set; }
        string DefinitionName { get; set; }
        string Description { get; set; }
        bool IsOnSale { get; }
        string Url { get; set; }
        bool IsNew { get; set; }
        IEnumerable<Item> Categories { get; set; }
        decimal Rating { get; set; }
        IEnumerable<Item> SortFields { get; set; }
        string ItemsPerPage { get; set; }
        string Variants { get; set; }
        string Brand { get; set; }
        string FirstImage { get; }
        string DefaultVariant { get; set; }
        VariantBox VariantBox { get; set; }
        VariantSize VariantSize { get; set; }
        UnitOfMeasure UnitOfMeasure { get; set; }
        IEnumerable<ProductTag> ProductTags { get; set; }
        string Image1 { get; set; }
        string Image2 { get; set; }
        string Image3 { get; set; }
        string MetalType { get; set; }
        string KaratPurity { get; set; }
        string Width { get; set; }
        string Style { get; set; }
        string MetalColor { get; set; }
        string InsideDimension { get; set; }
        string FabricationMethod { get; set; }
        string ShippingWeight { get; set; }
        string CountryOfOrigin { get; set; }
        List<string> AltImages();
    }
}