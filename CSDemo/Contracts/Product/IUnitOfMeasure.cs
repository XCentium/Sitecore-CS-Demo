using System;
using System.Collections.Generic;
using CSDemo.Models.Product;
using Sitecore.Data.Items;

namespace CSDemo.Contracts.Product
{
    public interface IUnitOfMeasure
    {
        Guid ID { get; set; }
        IEnumerable<UnitOfMeasureEnumeration> Units { get; set; }
    }
}