#region

using System;
using System.Web.Mvc;
using CSDemo.Models.Navigation;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

#endregion

namespace CSDemo.Controllers
{
    public class NavigationController : SitecoreController
    {
        #region Actions

        public ActionResult MainNavigation()
        {
            // TBD: fill up a view model
            // var datasourceItem = RenderingContext.Current;

            return View();
        }

        #endregion

        #region Fields

        private readonly ISitecoreContext _context;

        #endregion

        #region Constructors

        public NavigationController(ISitecoreContext context)
        {
            _context = context;
        }

        public NavigationController() : this(new SitecoreContext())
        {
        }

        #endregion
    }
}