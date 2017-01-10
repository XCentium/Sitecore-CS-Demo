using Glass.Mapper.Sc.Fields;

namespace CSDemo.Contracts.Article
{
    interface IArticleLinksData
    {
        #region Properties
        Glass.Mapper.Sc.Fields.Link ArticlePath { get; set; }
        Image LinkImage { get; set; }
        string LinkText { get; set; }
        string LinkTitle { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        #endregion


    }
}
