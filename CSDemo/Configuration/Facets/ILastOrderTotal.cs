using Sitecore.Analytics.Model.Framework;
using System;

namespace CSDemo.Configuration.Facets
{
    public interface ILastOrderTotal : IFacet
    {
        decimal Amount { get; set; }
        DateTime Updated { get; set; }
    }
}
