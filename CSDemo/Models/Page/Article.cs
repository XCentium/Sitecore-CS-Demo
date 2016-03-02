using CSDemo.Contracts.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Page
{
    [SitecoreType(AutoMap =true, Cachable = false)]
    public class Article :IArticle
    {
        public virtual string Title { get; set; }

        public virtual string Content { get; set; }
    }
}