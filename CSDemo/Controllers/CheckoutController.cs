#region

using CSDemo.Models.Cart;
using Sitecore;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;

#endregion

namespace CSDemo.Controllers
{
    public class CheckoutController : SitecoreController
    {
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart([CanBeNull] CartItem model)
        {

            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Address()
        {
            return View();
        }

        public ActionResult Shipping()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult Review()
        {
            return View();
        }


    }
}