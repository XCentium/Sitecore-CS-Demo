using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Blog
{
    public class BlogListViewModel : BlogExtraViewModel
    {

        public List<Blog> Blogs { get; set; }

        public BlogListViewModel()
        {
          Blogs = new List<Blog>();
        }
    }
}