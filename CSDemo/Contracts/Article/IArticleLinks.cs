using CSDemo.Models.Article;
using System.Collections.Generic;

namespace CSDemo.Contracts.Article
{
    interface IArticleLinks
    {
        #region Properties
        IEnumerable<ArticleLinksData> ArticlelinksList { get; set; }
        string Title { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion

    }
}
