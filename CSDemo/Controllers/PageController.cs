#region 

using CSDemo.Models.Page;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;

#endregion
namespace CSDemo.Controllers
{
    public class PageController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreContext _context;

        #endregion

        #region Constructors

        public PageController(ISitecoreContext context)
        {
            _context = context;
        }

        public PageController() : this(new SitecoreContext())
        {
        }

        #endregion
        
        public ActionResult Article()
        {
            var articleModel = _context.GetCurrentItem<Article>();
            return View(articleModel);
        }

        public ActionResult Metadata()
        {
            var contextItem = _context.GetCurrentItem<Item>();
            if (contextItem == null) return View();
            var metadataModel = contextItem.GlassCast<Metadata>();
            return View(metadataModel);
        }

        public ActionResult Header()
        {
            var homeItem = _context.GetHomeItem<Item>();
            if (homeItem == null) return View("~/Views/Page/Header.cshtml",null);
            var rootModel = homeItem.Parent.GlassCast<Root>();
            return View("~/Views/Page/Header.cshtml", rootModel);
        }
    }
}