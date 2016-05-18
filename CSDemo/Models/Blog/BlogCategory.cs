using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Contracts.Blog;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Models.Blog
{
    public class BlogCategory : IBlogCategory
    {

        private int myVar;

        public int TotalBlogs
        {
            get
            {
                int blogCount = BlogHelper.GetBlogCountByCategory(this.CategoryName);

                return blogCount; 
                
            }

        }
        

        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }
        [SitecoreField(Fields.CategoryName)]
        public virtual string CategoryName { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        #endregion
        #region Fields
        public struct Fields
        {
            public const string CategoryName = "Category Name";
        }
        #endregion
    }
}