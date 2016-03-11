#region

using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Contracts.Product
{
    interface IProductVariant
    {
        #region Properties
        string VariantId { get; set; }
        string ListPrice { get; set; }
        string Name { get; set; }

        IEnumerable<Item> Variant_Images { get; set; }
        string ProductSize { get; set; }
        string ProductColor { get; set; }

        #endregion 
    }
}
