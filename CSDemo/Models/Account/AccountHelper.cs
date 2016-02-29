using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Connect.CommerceServer;


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

                //var cxtUser = Sitecore.Context.User;
                //var ctxProfile = cxtUser.Profile;
                //var id3 = ctxProfile["user_id"];
                //var id4 = ctxProfile["user_id"];
                Tracker.Current.Session.Identify(userName);
                //var ctx2Profile = cxtUser.Profile;
                //var id5 = ctx2Profile["user_id"];

                CartHelper.MergeCarts(anonymousUserId);
            }

            return isLoggedIn;
        }

        public virtual CommerceUser GetUser(string userName)
        {
            var request = new GetUserRequest(userName);

            var result = this._customerServiceProvider.GetUser(request);

            CommerceUser usr = UpdateUserCustomerInfo(userName, result.CommerceUser);

            return result.CommerceUser;
        }
        /// <summary>
        /// Add essential customer properties to the CommerceUser
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="commerceUser"></param>
        /// <returns></returns>
        private CommerceUser UpdateUserCustomerInfo(string userName, CommerceUser commerceUser)
        {
            commerceUser.ExternalId = GetCommerceUserID(userName);

            if (!string.IsNullOrEmpty(commerceUser.ExternalId))
            {
                if (commerceUser.Customers == null || commerceUser.Customers.Count == 0)
                {
                    var customers = new List<string>() { commerceUser.ExternalId };
                    commerceUser.Customers = customers.AsReadOnly();
                }
            }

            return commerceUser;
        }

        internal string GetCommerceUserID(string userName)
        {
            // get sitecore account for the user
            //var user = UserManager.GetUsers().FirstOrDefault(usr => usr.Profile.Name.Equals(userName));

            
            string commerceUserID = string.Empty;

            var user = Sitecore.Security.Accounts.User.FromName(userName, true);
            Sitecore.Security.UserProfile profile = user.Profile;
            commerceUserID = user.Profile.GetCustomProperty("scommerce_customer_id");
            if (string.IsNullOrEmpty(commerceUserID))
            {
                commerceUserID = profile["user_id"];
                if (!string.IsNullOrEmpty(commerceUserID))
                {
                    // Create custom profile for scommerce_customer_id
                    // reload the profile
                    using (new Sitecore.Security.Accounts.UserSwitcher(user))
                    {
                        user.Profile.SetCustomProperty("scommerce_customer_id", commerceUserID);
                        user.Profile.Save();
                        user.Profile.Reload();
                    }
                }
            }                
                
   
            return commerceUserID;

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

        internal string GetAccountName(string userName)
        {
            // if email 
            if (userName.Contains("@") && !userName.Contains("\\"))
            {
                var matches = UserManager.GetUsers().Where(usr => usr.Profile.Email.Equals(userName)).ToList();
                if (matches != null)
                {
                    userName = matches[0].Name;
                }
            }

            var defaultDomain = Sitecore.Commerce.Connect.CommerceServer.Configuration.CommerceServerSitecoreConfig.Current.DefaultCommerceUsersDomain;
            if (string.IsNullOrWhiteSpace(defaultDomain))
            {
                defaultDomain = Sitecore.Commerce.Connect.CommerceServer.CommerceConstants.ProfilesStrings.CommerceUsersDomainName;
            }

            return !userName.StartsWith(defaultDomain, StringComparison.OrdinalIgnoreCase) ? string.Concat(defaultDomain, @"\", userName) : userName;

        }
    }
}