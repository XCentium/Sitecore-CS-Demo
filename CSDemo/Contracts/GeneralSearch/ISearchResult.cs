using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.GeneralSearch
{
    public interface ISearchResult
    {
        Image Image { get; set; }
        string Title { get; set; }
        double Price { get; set; }
        double Rating { get; set; }
        bool IsOnSale { get; set; }   
        bool IsNew { get; set; }
    }
}
