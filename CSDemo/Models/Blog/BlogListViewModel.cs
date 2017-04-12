using System.Collections.Generic;

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