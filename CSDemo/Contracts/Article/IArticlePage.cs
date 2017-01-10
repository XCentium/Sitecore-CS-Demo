
using CSDemo.Models.Article;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Article
{
    interface IArticlePage
    {
        #region Properties

        string Title { get; set; }
        string Content { get; set; }

        string Name { get; set; } 
        string Caption { get; set; } 

        string Url { get; set; }

        Image HeaderImage { get; set; }

        Accordion Accordion { get; set; }

        ArticleLinks Articlelink { get; set; }



        #endregion


    }
}
