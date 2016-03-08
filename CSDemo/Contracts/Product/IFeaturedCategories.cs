using System.Collections.Generic;
using CSDemo.Models.Product;

namespace CSDemo.Contracts.Product
{
    public interface IFeaturedCategories
    {
        IEnumerable<GeneralCategory> Categories { get; set; }
    }
}