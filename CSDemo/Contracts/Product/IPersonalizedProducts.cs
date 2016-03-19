using System.Collections.Generic;

namespace CSDemo.Contracts.Product
{
    public interface IPersonalizedProducts
    {
        IEnumerable<Models.Product.Product> Products { get; set; }
    }
}