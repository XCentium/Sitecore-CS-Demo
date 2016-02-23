using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Fields;
using Sitecore.Syndication;

namespace CSDemo.Contracts
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
