using System.Collections.Generic;
using Sitecore.Data.Items;
using XCore.Framework.ItemMapper;

namespace CSDemo.Contracts.Product
{
    public interface IFeaturedProducts
    {
        IEnumerable<Models.Product.Product> Products { get; set; }
    }
}
