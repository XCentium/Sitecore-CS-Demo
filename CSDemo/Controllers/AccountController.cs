#region

using System.Web.Mvc;
using CSDemo.Models.Account;
using Sitecore.Mvc.Controllers;
using CSDemo.Models.Product;
using CSDemo.Models.Checkout.Cart;

#endregion
namespace CSDemo.Controllers
{
    public class AccountController : SitecoreController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            // If already logged in, redirect to homepage
            if (Sitecore.Context.User.IsAuthenticated)
            {
                return this.Redirect(Constants.Common.ForwardSlash);
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignIn(LoginModel model)
        {

            if (ModelState.IsValid)
            {

                AccountHelper usr = new AccountHelper();

                var userName = usr.GetAccountName(model.Email);

                var uid = usr.GetCommerceUserId(userName);

                if (string.IsNullOrEmpty(uid))
                {
                    return this.Redirect(string.Format("{0}{1}", Sitecore.Context.Site.LoginPage, Constants.Account.SigninErrorMsg1));
                }

                if (usr.Login(userName, model.Password, model.RememberMe))
                {
                    return this.Redirect(Constants.Common.ForwardSlash);
                }

            }
            return this.Redirect(string.Format("{0}{1}", Sitecore.Context.Site.LoginPage, Constants.Account.SigninErrorMsg2));
        }

        public ActionResult SignOff()
        {
            // If already logged in, redirect to homepage
            if (Sitecore.Context.User.IsAuthenticated)
            {
                AccountHelper usr = new AccountHelper();
                usr.Logout();
            }
            return this.Redirect(Sitecore.Context.Site.LoginPage);
        }

 
        public ActionResult Account()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                return View();
            }
            return this.Redirect(Sitecore.Context.Site.LoginPage);

        }

        public ActionResult Orders()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var model = ProductHelper.GetCustomerOrders(new CartHelper());

                return View(model);
            }
            return this.Redirect(Sitecore.Context.Site.LoginPage);

        }

        public ActionResult OrderDetail()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var orderId = Sitecore.Web.WebUtil.GetUrlName(0);

                if (!string.IsNullOrEmpty(orderId))
                {
                    var cartHelper = new CartHelper();
                    var model = ProductHelper.GetCustomerOrderDetail(orderId, cartHelper);

                    // Only show the order detail if the viewer is the rightful owner
                    if (model.UserID == cartHelper.GetVisitorId())
                    {
                        return View(model);
                    }
                }

                return View();
            }
            return this.Redirect(Sitecore.Context.Site.LoginPage);

        }


        public ActionResult AddAddress()
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect(Sitecore.Context.Site.LoginPage); }
            var model = new Address();

            return View(model);
        }


        [HttpPost]
        public ActionResult AddAddress(Address model)
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect(Sitecore.Context.Site.LoginPage); }

            if (ModelState.IsValid)
            {
                var user = Sitecore.Context.User;

                AccountHelper acc = new AccountHelper();

                var result = acc.AddCustomerAddress(model);

                if (result == true) { return this.Redirect(Constants.Account.AddressLink); }

            }

            return View(model);
        }

        public ActionResult AddressDetail()
        {

            return View();
        }

        public ActionResult Addresses()
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect(Sitecore.Context.Site.LoginPage); }

            var model = new AccountHelper().GetCustomerAddresses();

            return View(model);

        }

    }
}