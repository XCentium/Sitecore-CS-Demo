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
using Sitecore.Analytics.Model.Entities;
using Sitecore.SecurityModel;
using Sitecore.Data.Items;
using System.IO;


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
                UpdateContactProfile();
                
                CartHelper.MergeCarts(anonymousUserId);
            }

            return isLoggedIn;
        }

        private void UpdateContactProfile()
        {
            if (Sitecore.Context.User.IsAuthenticated){

                Tracker.Current.Session.Identify(Sitecore.Context.User.Name);

                if (!string.IsNullOrEmpty(GetCommerceUserID(Sitecore.Context.User.Name)))
                {
                    var user = Sitecore.Context.User;
                    Sitecore.Security.UserProfile profile = user.Profile;

                    var userName = user.LocalName;
                    string userDomain = user.GetDomainName();

                    CommerceUser customer = GetUser(Sitecore.Context.User.Name);

                    if (customer.UserName != null)
                    {

                        if (Sitecore.Analytics.Tracker.Current.Contact != null)
                        {
                            // Update email information Faucet
                            var emailFacet = Tracker.Current.Contact.GetFacet<IContactEmailAddresses>("Emails");
                            //Check if an work email address already exists for the contact
                            if (!emailFacet.Entries.Contains("Personal Email"))
                            {
                                IEmailAddress email = emailFacet.Entries.Create("Personal Email");
                                email.SmtpAddress = customer.Email.Trim();

                            }
                            else
                            {
                                IEmailAddress email = emailFacet.Entries["Personal Email"];
                                email.SmtpAddress = customer.Email.Trim();
                            }
                            emailFacet.Preferred = "Personal Email";

                            // Update personal information Faucet
                            var personalFacet = Tracker.Current.Contact.GetFacet<IContactPersonalInfo>("Personal");

                            if (!string.IsNullOrEmpty(customer.FirstName) && !string.IsNullOrEmpty(customer.LastName))
                            {
                                //personalFacet.Title = "Name_Title";
                                personalFacet.FirstName = customer.FirstName;
                                //personalFacet.MiddleName = "Middle_Name";
                                personalFacet.Surname = customer.LastName;
                                //personalFacet.Gender = "Gender";
                                personalFacet.JobTitle = "Customer";
                                //personalFacet.BirthDate = new DateTime(1983, 01, 01);

                                using (new SecurityDisabler())
                                {
                                    profile.FullName = string.Format("{0} {1}", customer.FirstName, customer.LastName).Trim();
                                }
                                profile.Save();

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(profile.FullName.Trim()) && profile.FullName.Trim().Contains(" "))
                                {
                                    string[] names = profile.FullName.Trim().Split(' ');
                                    personalFacet.FirstName = names[0];
                                    personalFacet.Surname = names[1];
                                }
                                else
                                {
                                    personalFacet.FirstName = user.LocalName;
                                    personalFacet.Surname = "";
                                    profile.FullName = userName;
                                    profile.Save();
                                    user.Profile.Reload();
                                }

                            }

                            var pictureFacet = Tracker.Current.Contact.GetFacet<IContactPicture>("Picture");
                            string photoPath = "/sitecore/media library/CSDemo/Customers/" + user.LocalName;
                            MediaItem photoItem = Sitecore.Context.Database.GetItem(photoPath);

                            if (photoItem != null)
                            {
                                var stream = photoItem.GetMediaStream();
                                var memoryStream = new MemoryStream();
                                if (stream != null) { 
                                    stream.CopyTo(memoryStream);
                                    pictureFacet.Picture = memoryStream.ToArray();
                                    pictureFacet.MimeType = photoItem.MimeType;
                                }
                            }

                        }
                    }

                }

            }
            
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