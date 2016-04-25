using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Navigation
{
    public interface INavigationItem
    {
        Link Link { get; set; }
        string Path { get; set; }
        string MegaMenuTitle { get; set; }
        Image MegaMenuImage { get; set; }
    }
}