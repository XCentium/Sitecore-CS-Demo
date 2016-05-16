using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Blog
{
    public interface IBlogCategory
    {
        #region Properties
        string CategoryName { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion
    }
}
