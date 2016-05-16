using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Models.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Page
{
    public class HomePage
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }
        [SitecoreField(Fields.Categories)]
        public IEnumerable<Category> Categories { get; set; }
        [SitecoreField(Fields.MetaDescription)]
        public virtual string MetaDescription { get; set; }
        [SitecoreField(Fields.PageTitle)]
        public virtual string PageTitle { get; set; }
        [SitecoreField(Fields.Products)]
        public IEnumerable<Product.Product> Products { get; set; }
        [SitecoreField(Fields.FeaturedBlogs)]
        public IEnumerable<Blog.Blog> FeaturedBlogs { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string Categories = "Categories";
            public const string MetaDescription = "Meta Description";
            public const string PageTitle = "Page Title";
            public const string Products = "Products";
            public const string FeaturedBlogs = "Featured Blogs";
        }
        #endregion

    }
}