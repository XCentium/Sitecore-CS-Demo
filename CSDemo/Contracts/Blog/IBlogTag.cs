using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDemo.Contracts.Blog
{
    public interface IBlogTag
    {
        #region Properties
        string TagName { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion
    }
}
