using System;
using System.Collections.Generic;
using System.Linq;
using CSDemo.Configuration;
using CSDemo.Helpers;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Log = Sitecore.Diagnostics.Log;

namespace CSDemo.Models.Blog
{
    public static class BlogHelper
    {


        internal static List<BlogAuthor> GetAuthors()
        {
            var authors = new List<BlogAuthor>();
            const string authorsPath = Constants.Blog.AuthorsPath;
            var authorParent = Context.Database.GetItem(authorsPath);

            if (authorParent != null && authorParent.HasChildren)
            {
                authors.AddRange(authorParent.Children.Select(c => c.GlassCast<BlogAuthor>())); 
            }

            return authors;
        }

        internal static List<BlogCategory> GetCategories()
        {
            var categories = new List<BlogCategory>();
            const string categoriesPath = Constants.Blog.CategoriesPath;
            var categoriesParent = Context.Database.GetItem(categoriesPath);

            if (categoriesParent != null && categoriesParent.HasChildren)
            {
                categories.AddRange(categoriesParent.Children.Select(c => c.GlassCast<BlogCategory>())); //todo: refactor
            }

            return categories;
        }

        internal static List<BlogTag> GetTags()
        {
            var tags = new List<BlogTag>();
            const string tagsPath = Constants.Blog.TagsPath;
            var tagsParent = Context.Database.GetItem(tagsPath);

            if (tagsParent != null && tagsParent.HasChildren)
            {
                tags.AddRange(tagsParent.Children.Select(c => c.GlassCast<BlogTag>()));
            }

            return tags;
        }

        internal static int GetBlogCountByCategory(string categoryName)
        {

            var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
            try
            {
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);

                    var searchResultItem = queryable.Where(
                        x => x.TemplateName == Constants.Blog.BlogTemplate).ToList();

                    if (searchResultItem.Any())
                    {
                        var blogs = new List<Blog>();
                        blogs.AddRange(searchResultItem.Select(x=>x.GetItem().GlassCast<Blog>()));
                        var ctr = 0;

                        foreach (var blog in blogs)
                        {
                            if (blog.Category != null && blog.Category.Any())
                            {
                                var categories = blog.Category.Where(c => c.CategoryName.ToLower().Equals(categoryName.ToLower()));
                                if (categories.Any())
                                {
                                    ctr++;
                                }
                            }
                        }
                        return ctr;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);
            }

            return 0;

        }

        internal static List<Blog> GetBlogArticlesByName(string query)
        {
            var blogList = new List<Blog>();
            var blogs = GetSearchResultItemByNameOrId(query);

            if (blogs == null) return blogList;

            foreach (var searchResultItem in blogs)
            {
                try
                {
                    var blogItem = searchResultItem.Document.GetItem();
                    var blog = GlassHelper.Cast<Blog>(blogItem);

                    if (!string.IsNullOrWhiteSpace(blog.Title))
                    {
                        blogList.Add(new Blog { ID = blog.ID, Title = blog.Title, Url = blog.Url });
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.StackTrace, ex);
                }
            }

            return blogList;
        }

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
                                x.Path.Contains(Context.Site.StartPath + "/blogs") &&
                                x.TemplateName == "Blog Post").Page(0, 5).GetResults().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);
            }
            return null;
        }

    }
}