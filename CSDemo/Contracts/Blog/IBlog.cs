using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Glass.Mapper.Sc.Fields;
using CSDemo.Models.Blog;

namespace CSDemo.Contracts.Blog
{
    public interface IBlog
    {
        #region Properties
        IEnumerable<BlogCategory> Category { get; set; }
        Image Thumb { get; set; }
        IEnumerable<BlogAuthor> Author { get; set; }
        Image Main { get; set; }
        string Body { get; set; }
        IEnumerable<BlogTag> Tags { get; set; }
        string Summary { get; set; }
        DateTime PublishDate { get; set; }
        string Title { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion

    }
}
