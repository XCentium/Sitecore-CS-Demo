using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Configuration;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace CSDemo.Models.Blog
{
    public static class BlogHelper
    {


        internal static List<BlogAuthor> GetAuthors()
        {
            var authors = new List<BlogAuthor>();

            var authorsPath = Constants.Blog.AuthorsPath;

            Item authorParent = Sitecore.Context.Database.GetItem(authorsPath);

            if (authorParent != null && authorParent.HasChildren)
            {
                authors.AddRange(authorParent.Children.Select(c=>c.GlassCast<BlogAuthor>())); 
            }


            return authors;
        }

        internal static List<BlogCategory> GetCategories()
        {
            var categories = new List<BlogCategory>();

            var categoriesPath = Constants.Blog.CategoriesPath;

            Item categoriesParent = Sitecore.Context.Database.GetItem(categoriesPath);

            if (categoriesParent != null && categoriesParent.HasChildren)
            {
                categories.AddRange(categoriesParent.Children.Select(c => c.GlassCast<BlogCategory>()));
            }


            return categories;
        }

        internal static List<BlogTag> GetTags()
        {
            var tags = new List<BlogTag>();

            var tagsPath = Constants.Blog.TagsPath;

            Item tagsParent = Sitecore.Context.Database.GetItem(tagsPath);

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
                var culture = Context.Language.CultureInfo;
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
                        int ctr = 0;
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
    }
}