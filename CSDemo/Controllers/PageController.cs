using CSDemo.Models.Page;
using Glass.Mapper.Sc;
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
    }
}