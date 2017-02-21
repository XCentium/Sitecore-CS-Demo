using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Sitecore.Diagnostics;

namespace CSDemo.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)

        {
            Log.Info("Register custom routes", String.Empty);
            routes.MapRoute("productfoo", "foo/{controller}/{action}/{id}", 
                defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(

            //name: "Default",

            //url: "{controller}/{action}/{id}",

            //defaults: new
            //{
            //    controller = "Home",
            //    action = "Index",
            //    id = UrlParameter.Optional
            //}

            //);
        }
    }
}