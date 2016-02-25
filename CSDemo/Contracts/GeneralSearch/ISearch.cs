using System.Collections.Generic;

namespace CSDemo.Contracts.GeneralSearch
{
    public interface ISearch
    {
        string Query { get; set; }
        IList<ISearchResult> Results { get; set; }
        int PageSize { get; set; }
    }
}
