#region

using CSDemo.Models.Article;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace CSDemo.Controllers
{
    public class ArticleController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreContext _context;

        #endregion


        #region Constructors

        public ArticleController(ISitecoreContext context)
        {
            _context = context;
        }

        public ArticleController()
            : this(new SitecoreContext())
        {
        }

        #endregion


        public ActionResult ArticlePage()
        {
            var model = _context.GetCurrentItem<ArticlePage>();

            return View(model);
        }

        public ActionResult ArticleLinks()
        {
            var contextItem = _context.GetCurrentItem<Item>();

            Sitecore.Data.Fields.ReferenceField droplinkFld = contextItem.Fields["Article link"];

            Item articleLinksItem = droplinkFld.TargetItem;



            var model = articleLinksItem.GlassCast<ArticleLinks>();

            if (model.ArticlelinksList != null && model.ArticlelinksList.Any())
            {
                // Get all multilist items first
                Sitecore.Data.Items.Item[] items = null;

                Sitecore.Data.Fields.MultilistField multilistFld = articleLinksItem.Fields["Article links"];

                var articleLinksItemList = multilistFld.GetItems().ToList();

                var articleLinksList = articleLinksItemList.Select(x => x.GlassCast<ArticleLinksData>());

                //model = model.ArticlelinksList = articleLinksList.AsEnumerable();


                model.ArticlelinksList = model.ArticlelinksList.Select(x =>
                {
                    var arg = x;
                    x = articleLinksList.FirstOrDefault(y => y.ID == arg.ID);
                    return x;
                });

                //foreach (var source in model.ArticlelinksList.ToList())
                //{

                //}
            }

            return View(model);
        }

        public ActionResult Accordion()
        {

            var contextItem = _context.GetCurrentItem<Item>();

            Sitecore.Data.Fields.ReferenceField droplinkFld = contextItem.Fields["Accordion"];

            Item accordionItem = droplinkFld.TargetItem;

            var model = accordionItem.GlassCast<Accordion>();

            return View(model);
        }


    }
}