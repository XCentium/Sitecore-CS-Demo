#region

using SitecoreTypeAheadSearch.Configuration;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch.Linq;

#endregion

namespace SitecoreTypeAheadSearch.Models.Article
{
    public static class ArticleHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<SearchHit<SearchResultItem>> GetSearchResultItemByNameOrId(string query)
        {
            var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
            try
            {
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                            .Where(x => x.Language == Context.Language.Name);
              
                        return
                            queryable.Where(
                                x =>
                                    x.Name.Contains(query) &&
                                    x["_latestversion"] == "1" &&
                                    x.Path.Contains("/sitecore/content/storefront") &&
                                    x.TemplateName == "Article").Page(0, 5).GetResults().ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.StackTrace, ex);
            }
            return null;
        }

        internal static List<ArticleMini> GetArticlesByName(string query)
        {
            var articleList = new List<ArticleMini>();
            var articles = GetSearchResultItemByNameOrId(query);

            if (articles == null) return articleList;

            foreach (var searchResultItem in articles)
            {
                try
                {
                    var articleItem = searchResultItem.Document.GetItem();
                    var article = articleItem.GlassCast<Article>();
                    
                    if (!string.IsNullOrWhiteSpace(article.Title))
                    {
                        articleList.Add(new ArticleMini { Id = article.ID, Title = article.Title, Path = article.Path});
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error(ex.StackTrace, ex);
                }
            }

            return articleList;
        }
    }
}