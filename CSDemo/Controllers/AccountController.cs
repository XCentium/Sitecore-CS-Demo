using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSDemo.Models.Account;
using Sitecore.Mvc.Controllers;
using CSDemo.Models.Product;
using CSDemo.Models.Checkout.Cart;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Services.Customers;

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
                return this.Redirect("/");
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

                var uid = usr.GetCommerceUserID(userName);
                if (string.IsNullOrEmpty(uid))
                {
                    return this.Redirect("/account/signin?msg=Only Commerce Customers Allowed to Signin");
                }

                if (usr.Login(userName, model.Password, model.RememberMe))
                {
                    return this.Redirect("/");
                }

            }
            return this.Redirect("/account/signin?msg=Incorrect Username or Password");
        }

        public ActionResult SignOff()
        {
            // If already logged in, redirect to homepage
            if (Sitecore.Context.User.IsAuthenticated)
            {
                AccountHelper usr = new AccountHelper();
                usr.Logout();
            }
            return this.Redirect("/account/signin");
        }


        public ActionResult Account()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                return View();
            }
            return this.Redirect("/account/signin");


        }

        public ActionResult Orders()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var model = ProductHelper.GetCustomerOrders(new CartHelper());

                return View(model);
            }
            return this.Redirect("/account/signin");

        }

        public ActionResult OrderDetail()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var orderID = Sitecore.Web.WebUtil.GetUrlName(0);

                if (!string.IsNullOrEmpty(orderID))
                {
                    var cartHelper = new CartHelper();
                    var model = ProductHelper.GetCustomerOrderDetail(orderID, cartHelper);

                    // Only show the order detail if the viewer is the rightful owner
                    if (model.UserID == cartHelper.GetVisitorID())
                    {
                        return View(model);
                    }
                }

                return View();
            }
            return this.Redirect("/account/signin");

        }


        public ActionResult AddAddress()
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect("/account/signin"); }
            var model = new Address();

            return View(model);
        }


        [HttpPost]
        public ActionResult AddAddress(Address model)
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect("/account/signin"); }

            if (ModelState.IsValid)
            {
                var user = Sitecore.Context.User;

                AccountHelper acc = new AccountHelper();

                var result = acc.AddCustomerAddress(model);

                if (result == true) { return this.Redirect("/account/Addresses"); }

            }

            return View(model);
        }

        public ActionResult AddressDetail()
        {

            return View();
        }

        public ActionResult Addresses()
        {
            if (!Sitecore.Context.User.IsAuthenticated) { return this.Redirect("/account/signin"); }

            var model = new AccountHelper().GetCustomerAddresses();

            return View(model);

        }

    }
}