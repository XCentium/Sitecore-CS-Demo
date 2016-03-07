#region

using System;
using System.Collections.Generic;
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
            if (!datasource.IsEmptyOrNull())
            {
                var parentItem = _context.Database.GetItem(datasource);
                if (parentItem == null || !parentItem.HasChildren) return View(navigationItems);
                var children = parentItem.Children;
                foreach (Item child in children)
                {
                    var navigationItem = child.GlassCast<NavigationItem>();
                    if (navigationItem != null)
                    {
                        navigationItems.Add(navigationItem);
                    }
                }
            }
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