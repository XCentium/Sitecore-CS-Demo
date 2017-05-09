using CSDemo.Contracts.Blog;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Blog
{
    [SitecoreType(AutoMap = true)]
    public class FeaturedBlogs : IFeaturedBlogs
    {
        [SitecoreField(Fields.FeaturedBlogs)]
        public virtual IEnumerable<Blog> Blogs { get; set; }

        public struct Fields
        {
            public const string FeaturedBlogs = "Featured Blogs";
        }
    }
}