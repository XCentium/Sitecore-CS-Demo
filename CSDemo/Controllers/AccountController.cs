#region

using System.Linq;
using System.Web.Mvc;
using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore;
using Sitecore.Mvc.Controllers;
using Sitecore.Web;
using Address = CSDemo.Models.Account.Address;

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
            if (Context.User.IsAuthenticated)
            {
                return Redirect(Constants.Common.ForwardSlash);
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignIn(LoginModel model)
        {

            if (ModelState.IsValid)
            {

                var usr = new AccountHelper();
                var userName = usr.GetAccountName(model.Email);

                var uid = usr.GetCommerceUserId(userName);

                if (string.IsNullOrEmpty(uid))
                {
                    return Redirect(string.Format("{0}{1}", Context.Site.LoginPage, Constants.Account.SigninErrorMsg1));
                }

                if (usr.Login(userName, model.Password, model.RememberMe))
                {
                    return Redirect(Constants.Common.ForwardSlash);
                }

            }
            return Redirect(string.Format("{0}{1}", Context.Site.LoginPage, Constants.Account.SigninErrorMsg2));
        }

        public ActionResult SignOff()
        {
            // If already logged in, redirect to homepage
            if (Context.User.IsAuthenticated)
            {
                var usr = new AccountHelper();
                usr.Logout();
            }
            return Redirect(Context.Site.LoginPage);
        }


        public ActionResult Account()
        {
            if (Context.User.IsAuthenticated)
            {
                return View();
            }
            return Redirect(Context.Site.LoginPage);

        }

        public ActionResult Orders()
        {
            if (Context.User.IsAuthenticated)
            {
                var model = ProductHelper.GetCustomerOrders(new CartHelper());

                return View(model);
            }
            return Redirect(Context.Site.LoginPage);

        }

        public ActionResult OrderDetail()
        {
            if (!Context.User.IsAuthenticated) return Redirect(Context.Site.LoginPage);

            var orderId = WebUtil.GetUrlName(0);

            if (string.IsNullOrEmpty(orderId)) return View();

            orderId = orderId.Replace(" ", "-");
            var cartHelper = new CartHelper();
            var model = ProductHelper.GetCustomerOrderDetail(orderId, cartHelper);

            // Only show the order detail if the viewer is the rightful owner
            return model.UserId == cartHelper.GetVisitorId() ? View(model) : View();
        }


        public ActionResult AddAddress()
        {
            if (!Context.User.IsAuthenticated) { return Redirect(Context.Site.LoginPage); }
            var model = new Address();

            return View(model);
        }


        [HttpPost]
        public ActionResult AddAddress(Address model)
        {
            if (!Context.User.IsAuthenticated) { return Redirect(Context.Site.LoginPage); }

            if (ModelState.IsValid)
            {
                var acc = new AccountHelper();
                var result = acc.AddCustomerAddress(model);

                if (result) { return Redirect(Constants.Account.AddressLink); }

            }

            return View(model);
        }

        public ActionResult AddressDetail()
        {
            Models.Address model;

            var id = Request.QueryString["id"];

            if (!string.IsNullOrEmpty(id))
            {
                model = AccountHelper.GetUserAddresses().FirstOrDefault(x => x.AddressID == id);
            }
            else
            {
                model = new Models.Address();
            }

            return View(model);
        }

        public ActionResult Addresses()
        {
            if (!Context.User.IsAuthenticated) { return Redirect(Context.Site.LoginPage); }

            var model = AccountHelper.GetUserAddresses();

            return View(model);

        }

    }
}