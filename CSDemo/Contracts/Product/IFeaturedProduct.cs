using System.Collections.Generic;
using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;

namespace CSDemo.Contracts.Product
{
    public interface IFeaturedProduct
    {
        IEnumerable<Models.Product.Product> Products { get; set; }
    }
}
