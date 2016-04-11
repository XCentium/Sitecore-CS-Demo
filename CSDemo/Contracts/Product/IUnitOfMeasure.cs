using System.Collections.Generic;
using CSDemo.Models.Product;

namespace CSDemo.Contracts.Product
{
    public interface IUnitOfMeasure
    {
        IEnumerable<UnitOfMeasureEnumeration> Enumerations { get; set; }
    }
}