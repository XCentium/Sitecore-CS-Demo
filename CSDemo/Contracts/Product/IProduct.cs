using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Product
{
    public interface IProduct
    {
        string Title { get; set; }
        string ProductId { get; set; }
        DateTime DateOfIntroduction { get; set; }
        string Size { get; set; }
        IEnumerable<Image> Images { get; set; }
        string CatalogName { get; set; }
        decimal Price { get; set; }
        string DefinitionName { get; set; }
        string Description { get; set; }
        bool IsOnSale { get; set; }
        string Url { get; set; }
    }
}
