using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDemo.Contracts.Page
{
    public interface IArticle
    {
        string Title { get; set; }
        string Content { get; set; }
    }
}
