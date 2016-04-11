using System;

namespace CSDemo.Contracts.Product
{
    public interface IUnitOfMeasureEnumeration
    {
        Guid ID { get; set; }
        string Value { get; set; }
    }
}