using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDemo.Configuration.Facets
{
    public interface ILastOrderTotal : IFacet
    {
        decimal Amount { get; set; }
        DateTime Updated { get; set; }
    }
}
