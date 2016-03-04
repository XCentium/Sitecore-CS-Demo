#region

using System;
using System.Web.Mvc;
using Sitecore.Mvc.Controllers;

#endregion

namespace CSDemo.Controllers
{
    public class NavigationController : SitecoreController
    {
        public ActionResult MainNavigation()
        {
            return View();
        }
    }
}