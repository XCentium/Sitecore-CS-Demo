using System.Collections.Generic;
using Sitecore.Data.Items;

namespace CSDemo.Contracts.Navigation
{
    public interface IMegaMenuNavigationItem
    {
        string Title { get; set; }
        IEnumerable<Item> Links { get; set; }
    }
}