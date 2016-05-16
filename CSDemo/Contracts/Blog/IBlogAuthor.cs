using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Blog
{
   
    public interface IBlogAuthor
    {
        #region Properties
        string Title { get; set; }
        string EmailAddress { get; set; }
        string Creator { get; set; }
        Image ProfileImage { get; set; }
        string FullName { get; set; }
        string Bio { get; set; }
        string Location { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion

    }
}
