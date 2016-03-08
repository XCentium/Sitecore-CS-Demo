#region

using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Contracts.Product
{
    public interface IGeneralCategory
    {
        Guid ID { get; set; }
        string Title { get; set; }
        string Brand { get; set; }
        string Description { get; set; }
        IEnumerable<Item> Images { get; set; }
    }
}