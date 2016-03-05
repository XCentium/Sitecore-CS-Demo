using System.Collections.Generic;
using CSDemo.Models.Navigation;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Navigation
{
    public interface IMainNavigation
    {
        Image MegaMenuImage { get; set; }
        IEnumerable<NavigationItem> NavigationItems { get; set; }
    }
}