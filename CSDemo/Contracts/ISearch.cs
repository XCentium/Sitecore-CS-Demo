using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSDemo.Models.Search;

namespace CSDemo.Contracts
{
    public interface ISearch
    {
        string Query { get; set; }
        IList<ISearchResult> Results { get; set; }
        int PageSize { get; set; }
    }
}
