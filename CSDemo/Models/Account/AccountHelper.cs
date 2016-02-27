using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class AccountHelper
    {
        public CartHelper CartHelper { get; set; }
        private readonly CustomerServiceProvider _customerServiceProvider;
        public AccountHelper()
        {
            CartHelper = new CartHelper();
            _customerServiceProvider = new CustomerServiceProvider();
        }

        public AccountHelper(CartHelper cartHelper, CustomerServiceProvider customerServiceProvider)
        {
            this.CartHelper = cartHelper;
            this._customerServiceProvider = customerServiceProvider;
        }

        public virtual bool Login(string userName, string password, bool persistent)
        {
            string anonymousUserId = CartHelper.GetVisitorID();

            var isLoggedIn = AuthenticationManager.Login(userName, password, persistent);
            if (isLoggedIn)
            {

                Tracker.Current.Session.Identify(userName);

                CartHelper.MergeCarts(anonymousUserId);
            }

            return isLoggedIn;
        }

        public virtual CommerceUser GetUser(string userName)
        {
            var request = new GetUserRequest(userName);

            var result = this._customerServiceProvider.GetUser(request);

            return result.CommerceUser;
        }


        /// <summary>
        /// Gets a customer based on an id
        /// </summary>
        /// <param name="id">The customer to retrieve</param>
        /// <returns>The requested customer</returns>
        public virtual CommerceCustomer GetCustomer(string id)
        {
            var request = new GetCustomerRequest(id);

            var result = this._customerServiceProvider.GetCustomer(request);

            return result.CommerceCustomer;
        }

        /// <summary>
        /// Logouts the current user.
        /// </summary>
        public virtual void Logout()
        {
            Tracker.Current.EndVisit(true);
            System.Web.HttpContext.Current.Session.Abandon();
            AuthenticationManager.Logout();
        }
    }
}