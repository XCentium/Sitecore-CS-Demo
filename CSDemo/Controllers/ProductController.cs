using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSDemo.Models;
using Sitecore.Mvc.Controllers;

namespace CSDemo.Controllers
{
    public class ProductController : SitecoreController
    {
        public ActionResult ProductDetails()
        {
            return View(new Product());
        }
    }
}