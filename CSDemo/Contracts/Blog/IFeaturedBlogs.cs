using System.Collections.Generic;

namespace CSDemo.Contracts.Blog
{
    public interface IFeaturedBlogs
    {
        IEnumerable<CSDemo.Models.Blog.Blog> Blogs { get; set; }
    }
}

