using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSDemo.Models.Account;
using Sitecore.Mvc.Controllers;

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
                if (string.IsNullOrEmpty(uid)) {
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
                 AccountHelper usr = new AccountHelper();

                 var orders = usr.GetOrders();

                 return View();
             }
             return this.Redirect("/account/signin");

         }

         public ActionResult OrderDetail()
         {
             if (Sitecore.Context.User.IsAuthenticated)
             {
                 AccountHelper usr = new AccountHelper();

                 return View();
             }
             return this.Redirect("/account/signin");

         }

    }
}