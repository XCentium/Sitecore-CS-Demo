#region

using Sitecore;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;
using CSDemo.Models.Checkout.Cart;

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

        public ActionResult Reviewx()
        {
            return View();
        }

        public ActionResult Reviewy()
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

        #region Testing

        public ActionResult ReviewNoHeaderFooter()
        {
            return View();
        }

        #endregion
    }
}