using CSDemo.Models.Page;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var articleModel = contextItem.GlassCast<Metadata>();
            return View(articleModel);
        }

        public ActionResult Header()
        {
            return View("~/Views/Page/Header.cshtml");
        }
    }
}