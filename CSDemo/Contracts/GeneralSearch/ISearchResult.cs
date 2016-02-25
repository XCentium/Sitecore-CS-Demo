using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;


namespace CSDemo.Contracts.GeneralSearch
{
    public interface ISearchResult
    {
        IEnumerable<Image> Images { get; set; }
        string Title { get; set; }
        decimal Price { get;  }
        double Rating { get; set; }
        bool IsOnSale { get; set; }   
        bool IsNew { get; set; }
        string Url { get; set; }
    }
}
