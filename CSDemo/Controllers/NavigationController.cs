#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Models.Navigation;
using Glass.Mapper.Sc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;

#endregion

namespace CSDemo.Controllers
{
    public class NavigationController : SitecoreController
    {
        #region Actions

        public ActionResult MainNavigation()
        {
            List<NavigationItem> navigationItems = new List<NavigationItem>();

            // TBD: fill up a view model
            var datasource = RenderingContext.Current.Rendering.DataSource;
            if (datasource.IsEmptyOrNull()) return View(navigationItems);
            var parentItem = _context.Database.GetItem(datasource);
            if (parentItem == null || !parentItem.HasChildren) return View(navigationItems);
            var children = parentItem.Children.ToList();
            navigationItems.AddRange(children.Select(child => child.GlassCast<NavigationItem>()).Where(navigationItem => navigationItem != null));
            return View(navigationItems);
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