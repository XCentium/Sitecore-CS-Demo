using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Models.Page;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using CSDemo.Models.Blog;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace CSDemo.Controllers
{
    public class BlogController : SitecoreController
    {
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlogList()
        {
            var model = new BlogListViewModel
            {
                Authors = BlogHelper.GetAuthors(),
                Categories = BlogHelper.GetCategories(),
                Tags = BlogHelper.GetTags()
            };


            var contextItem = Sitecore.Context.Item;

            if (contextItem != null)
            {
                var blogList = contextItem.Axes.GetDescendants()
                    .AsQueryable()
                    .Where(x => x.TemplateName == Constants.Blog.BlogTemplate).ToList();

                if (blogList.Any())
                {
                    var blogs = blogList.Select(b => b.GlassCast<Blog>()).ToList();

                    var secondPart = WebUtil.GetUrlName(0);
                    var firstPart = WebUtil.GetUrlName(1);


                    if (!string.IsNullOrEmpty(firstPart) && !string.IsNullOrEmpty(secondPart))
                    {

                        if (firstPart.ToLower().Equals(Constants.Blog.Category))
                        {
                            foreach (var blog in blogs)
                            {
                                if (blog.Category != null && blog.Category.Any())
                                {
                                    var categories = blog.Category.Where(c=>c.CategoryName.ToLower().Equals(secondPart.ToLower()));
                                    if (categories.Any())
                                    {
                                        model.Blogs.Add(blog);
                                    }
                                }
                            }
                        }

                        if (firstPart.ToLower().Equals(Constants.Blog.Tag))
                        {
                            foreach (var blog in blogs)
                            {
                                if (blog.Tags != null && blog.Tags.Any())
                                {
                                    var tags = blog.Tags.Where(t=>t.TagName.ToLower().Equals(secondPart.ToLower()));
                                    if (tags.Any())
                                    {
                                        model.Blogs.Add(blog);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        model.Blogs.AddRange(blogs.Select(x => x));
                    }
                }

            }

            return View(model);
        }

        public ActionResult BlogPost()
        {
            var model = new BlogViewModel
            {
                Authors = BlogHelper.GetAuthors(),
                Categories = BlogHelper.GetCategories(),
                Tags = BlogHelper.GetTags()
            };

            var contextItem = Sitecore.Context.Item;

            if (contextItem != null)
            {
                model.Blog = contextItem.GlassCast<Blog>();
            }

                return View(model);
        }

        public ActionResult FeaturedBlogs()
        {
            var model = new List<Blog>();

            var renderingItem = RenderingContext.Current.Rendering.Item; // csdemo >> home


            if (renderingItem != null)
            {
                var homePage = renderingItem.GlassCast<HomePage>();

                if (homePage != null)
                {
                    var featuredBlogs = homePage.FeaturedBlogs.ToList();
                    if (featuredBlogs.Any())
                    {
                        model = featuredBlogs;
                    }
                }
            }

            return View(model);
        }
    }
}