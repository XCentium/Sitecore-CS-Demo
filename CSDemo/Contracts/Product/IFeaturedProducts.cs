using System.Collections.Generic;

namespace CSDemo.Contracts.Product
{
    public interface IFeaturedProducts
    {
        IEnumerable<Models.Product.Product> Products { get; set; }
    }
}