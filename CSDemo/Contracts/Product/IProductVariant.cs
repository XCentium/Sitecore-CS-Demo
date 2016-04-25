#region

using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Sitecore.Commerce.Entities.Inventory;

#endregion

namespace CSDemo.Contracts.Product
{
    interface IProductVariant
    {
        #region Properties
        string VariantId { get; set; }
        string ListPrice { get; set; }
        string Name { get; set; }

        IEnumerable<Image> Images { get; set; }
        string ProductSize { get; set; }
        string ProductColor { get; set; }

        StockInformation StockInformation { get; set; }
        string StockLabel { get; set; }
        int StockQuantity { get; set; }

        string Image1 { get; set; }
        string Image2 { get; set; }
        string Image3 { get; set; }
        List<string> AltImages();
        #endregion 
    }
}
