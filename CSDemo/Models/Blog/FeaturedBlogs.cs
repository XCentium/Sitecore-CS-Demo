using CSDemo.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Blog
{
    [SitecoreType(AutoMap = true)]
    public class FeaturedBlogs : IFeaturedBlogs
    {
        public virtual IEnumerable<Blog> Blogs { get; set; }
    }
}