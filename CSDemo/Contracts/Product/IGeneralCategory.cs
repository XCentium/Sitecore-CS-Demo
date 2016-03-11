#region

using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

#endregion

namespace CSDemo.Contracts.Product
{
    public interface IGeneralCategory 
    {
        string Title { get; set; }
        string Brand { get; set; }
        string Description { get; set; }
        IEnumerable<Image> Images { get; set; }
    }
}