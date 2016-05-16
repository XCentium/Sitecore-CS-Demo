using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSDemo.Models.Product;
using Sitecore.Data.Items;

namespace CSDemo.Contracts.Page
{
    public interface IHomePage
    {
        #region Properties
        IEnumerable<Category> Categories { get; set; }
        string MetaDescription { get; set; }
        string PageTitle { get; set; }
        IEnumerable<Models.Product.Product> Products { get; set; }
        IEnumerable<Models.Blog.Blog> FeaturedBlogs { get; set; }
        string Name { get; set; }
        string Url { get; set; }

        #endregion
    }
}
