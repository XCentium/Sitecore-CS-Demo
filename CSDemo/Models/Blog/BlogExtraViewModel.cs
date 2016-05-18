using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Blog
{
    public class BlogExtraViewModel
    {
        public List<BlogAuthor> Authors { get; set; }

        public List<BlogCategory> Categories { get; set; }
        public List<BlogTag> Tags { get; set; }


        public BlogExtraViewModel()
        {
            Authors = new List<BlogAuthor>();
            Categories = new List<BlogCategory>();
            Tags = new List<BlogTag>();
        }
    }
}