#region 

using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Analytics.Model.Entities;
using Sitecore.SecurityModel;
using Sitecore.Data.Items;
using System.IO;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Diagnostics;
using System.Collections.ObjectModel;
using CSDemo.Models.Product;

#endregion

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

        /// <summary>
        /// Log a Commerceuser in
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="persistent"></param>
        /// <returns></returns>
        public virtual bool Login(string userName, string password, bool persistent)
        {
            string anonymousUserId = CartHelper.GetVisitorId();

            var isLoggedIn = AuthenticationManager.Login(userName, password, persistent);

            if (isLoggedIn)
            {
                UpdateContactProfile();
                
                CartHelper.MergeCarts(anonymousUserId);
            }

            SetUserCatalogCookie();

            return isLoggedIn;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetUserCatalogCookie()
        {

            Cookie.Set("CommerceUserLoggedIn", "true");

            var catalogOptions = GetLoggedInCustomerCatalogOptions();

            if (!string.IsNullOrEmpty(catalogOptions))
            {
                Cookie.Set("UserCatalogOptions", catalogOptions);

                // if more than one set the cookie options
                if (catalogOptions.Contains(Constants.Common.PipeStringSeparator))
                {
                    Cookie.Set("UserSelectedCatalogId", "");
                }
                else
                {
                    // if it is just one catalog, get the ID and set the cookie
                    SetUserCatalogChoice(catalogOptions);

                }
            }
        }


        internal void SetUserCatalogChoice(string catalogName)
        {
            // if it is just one catalog, get the ID and set the cookie
            var catalogItem = ProductHelper.GetCatalogItemByName(catalogName);
            if (catalogItem != null)
            {
                Cookie.Set("UserSelectedCatalogId", catalogItem.ID.ToString());
            }
        }

        /// <summary>
        /// Update a user profile in Sitecore
        /// </summary>
        private void UpdateContactProfile()
        {
            if (Sitecore.Context.User.IsAuthenticated){

                Tracker.Current.Session.Identify(Sitecore.Context.User.Name);

                if (!string.IsNullOrEmpty(GetCommerceUserId(Sitecore.Context.User.Name)))
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
                            var emailFacet = Tracker.Current.Contact.GetFacet<IContactEmailAddresses>(Constants.Account.FacetEmail);

                            //Check if an work email address already exists for the contact
                            if (!emailFacet.Entries.Contains(Constants.Account.PersonalEmail))
                            {
                                IEmailAddress email = emailFacet.Entries.Create(Constants.Account.PersonalEmail);
                                email.SmtpAddress = customer.Email.Trim();

                            }
                            else
                            {
                                IEmailAddress email = emailFacet.Entries[Constants.Account.PersonalEmail];
                                email.SmtpAddress = customer.Email.Trim();
                            }
                            emailFacet.Preferred = Constants.Account.PersonalEmail;

                            // Update personal information Faucet
                            var personalFacet = Tracker.Current.Contact.GetFacet<IContactPersonalInfo>(Constants.Account.FacetPersonal);

                            if (!string.IsNullOrEmpty(customer.FirstName) && !string.IsNullOrEmpty(customer.LastName))
                            {
                                //personalFacet.Title = "Name_Title";
                                personalFacet.FirstName = customer.FirstName;
                                //personalFacet.MiddleName = "Middle_Name";
                                personalFacet.Surname = customer.LastName;
                                //personalFacet.Gender = "Gender";
                                personalFacet.JobTitle = Constants.Account.JobTitle;
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

                            var pictureFacet = Tracker.Current.Contact.GetFacet<IContactPicture>(Constants.Account.FacetPicture);
                            string photoPath = Constants.Account.CustomerPhotoPath + user.LocalName;
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

        /// <summary>
        /// Get Commerce user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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
            commerceUser.ExternalId = GetCommerceUserId(userName);

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

        /// <summary>
        /// Get CommerceuserID of a customer
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal string GetCommerceUserId(string userName)
        {
  
            var user = User.FromName(userName, true);
            Sitecore.Security.UserProfile profile = user.Profile;
            var commerceUserId = user.Profile.GetCustomProperty(Constants.Commerce.CommerceCustomerId);
            if (string.IsNullOrEmpty(commerceUserId))
            {
                commerceUserId = profile[Constants.Commerce.CommerceUserId];
                if (!string.IsNullOrEmpty(commerceUserId))
                {
                    // Create custom profile for scommerce_customer_id
                    // reload the profile
                    using (new Sitecore.Security.Accounts.UserSwitcher(user))
                    {
                        user.Profile.SetCustomProperty(Constants.Commerce.CommerceCustomerId, commerceUserId);
                        user.Profile.Save();
                        user.Profile.Reload();
                    }
                }
            }                
                
   
            return commerceUserId;

        }

        internal string GetLoggedInCustomerCatalogOptions()
        {

            if (Sitecore.Context.User.IsAuthenticated)
            {
                string userName = Sitecore.Context.User.Name;
                var user = User.FromName(userName, true);
                Sitecore.Security.UserProfile profile = user.Profile;

                var commerceCatalogSet = profile[Constants.Commerce.CommerceUserCatalogSetId];

  //              commerceCatalogSet = "{22222222-2222-2222-2222-222222222222}";

                if (!string.IsNullOrEmpty(commerceCatalogSet) && commerceCatalogSet.Contains(Constants.Common.Dash))
                {
                    return GetCatalogNamesFromCatalogSet(commerceCatalogSet);
                }

            }


            return String.Empty;
        }


        private string GetCatalogNamesFromCatalogSet(string commerceCatalogSet)
        {
            if (!string.IsNullOrEmpty(commerceCatalogSet))
            {

                var commerceCatalogSetItem = Sitecore.Context.Database.GetItem(commerceCatalogSet);
                if (commerceCatalogSetItem != null)
                {
                    var commerceCatalogNames = commerceCatalogSetItem[Constants.Account.CatalogSetField];

                    if (!string.IsNullOrEmpty(commerceCatalogNames))
                    {
                       return commerceCatalogNames;
                    }
                }

            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the catalogIDs that should be used to load the current user's product.
        /// </summary>
        /// <returns></returns>
        internal string GetCurrentCustomerCatalogIds()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {
                var cataLogId = Cookie.Get("UserSelectedCatalogId").Value;

                if (cataLogId != null && !string.IsNullOrEmpty(cataLogId))
                {
                    return cataLogId;
                }

                string userName = Sitecore.Context.User.Name;
                var user = User.FromName(userName, true);
                Sitecore.Security.UserProfile profile = user.Profile;

                var commerceCatalogSet = profile[Constants.Commerce.CommerceUserCatalogSetId];

                if (!string.IsNullOrEmpty(commerceCatalogSet) && commerceCatalogSet.Contains(Constants.Common.Dash))
                {
                    return GetCatalogIdsFromCatalogSet(commerceCatalogSet); 
                }

            }

           return ProductHelper.GetSiteRootCatalogId();

        }

        private string GetCatalogIdsFromCatalogSet(string commerceCatalogSet)
        {
            if (!string.IsNullOrEmpty(commerceCatalogSet))
            {

                var commerceCatalogSetItem = Sitecore.Context.Database.GetItem(commerceCatalogSet);
                if (commerceCatalogSetItem != null)
                {
                    var commerceCatalogNames = commerceCatalogSetItem[Constants.Account.CatalogSetField];

                    if (!string.IsNullOrEmpty(commerceCatalogNames))
                    {
                        var catalogIds = new List<string>();
                        string[] catalogNames = commerceCatalogNames.Split(Constants.Common.PipeSeparator);

                        foreach (var catalogName in catalogNames)
                        {
                            var catalogItem = ProductHelper.GetCatalogItemByName(catalogName);
                            if (catalogItem != null) catalogIds.Add(catalogItem.ID.ToString());
                        }

                        return catalogIds.Aggregate((current, next) => current + Constants.Common.PipeStringSeparator + next);
                    }
                }
               
            }
            return string.Empty;
        }




        /// <summary>
        /// Gets a customer based on an id
        /// </summary>
        /// <param name="userExternalId">The customer to retrieve</param>
        /// <returns>The requested customer</returns>
        public virtual CommerceCustomer GetCustomer(string userExternalId)
        {
            var request = new GetCustomerRequest(userExternalId);

            var result = this._customerServiceProvider.GetCustomer(request);

            if (result.Success)
            {
                return result.CommerceCustomer;
            }

            throw new ApplicationException(result.SystemMessages.Any() ? result.SystemMessages[0].Message : Constants.Account.Error);

        }

        /// <summary>
        /// Logouts the current user.
        /// </summary>
        public virtual void Logout()
        {
            Tracker.Current.EndVisit(true);
            System.Web.HttpContext.Current.Session.Abandon();
            AuthenticationManager.Logout();
            ClearUserCatalogCookies();
        }

        private void ClearUserCatalogCookies()
        {
            Cookie.Del("CommerceUserLoggedIn");
            Cookie.Del("UserCatalogOptions");
            Cookie.Del("UserSelectedCatalogId");
            Cookie.Del("ShowCoupon");
            Cookie.Del("CouponMessage");
        }

        /// <summary>
        /// Get Account Name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add Customer Address
        /// </summary>
        /// <param name="customerAddress"></param>
        /// <returns></returns>
        public virtual bool AddCustomerAddress(Address customerAddress)
        {


            CommerceUser commerceUser = this.GetUser(Sitecore.Context.User.Name);

            if (commerceUser.UserName != null)
            {
                var user = Sitecore.Security.Accounts.User.FromName(Sitecore.Context.User.Name, true);
                Sitecore.Security.UserProfile profile = user.Profile;

                var created = DateTime.Now;

                var customer = new CommerceCustomer { ExternalId = commerceUser.ExternalId };
                var party = new CommerceParty
                {
                    ExternalId = Guid.NewGuid().ToString("B"),
                    Name = customerAddress.AddressName + DateTime.Now.ToShortTimeString(),
                    Address1 = customerAddress.Address1,
                    Address2 = customerAddress.Address2,
                    PhoneNumber = customerAddress.Phone,
                    FaxNumber = customerAddress.Fax,
                    City = customerAddress.City,
                    Country = customerAddress.Country,
                    State = customerAddress.State,
                    ZipPostalCode = customerAddress.Zip,
                    PartyId = customerAddress.PartyId,
                    IsPrimary = customerAddress.IsMain
                };

                try
                {

                    var newCustomerName = string.Format("{0}.{1}.{2}.{3}", customerAddress.Company.Trim(), customerAddress.LastName.Trim(), customerAddress.FirstName.Trim(), created);

                    CommerceCustomer newCustomer = CreateCustomer(customer, newCustomerName, party, party, Sitecore.Context.User.LocalName);

                }
                catch (Exception)
                {

                    return false;
                    throw;

                }

            }

            return true;
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="emptyCustomer"></param>
        /// <param name="newCustomerName"></param>
        /// <param name="billToParty"></param>
        /// <param name="shipToParty"></param>
        /// <param name="loggedInUserName"></param>
        /// <returns></returns>
        private CommerceCustomer CreateCustomer(CommerceCustomer emptyCustomer, string newCustomerName, CommerceParty billToParty, CommerceParty shipToParty, string loggedInUserName)
        {
            var cust = new CustomerServiceProvider();
            var customer = new CommerceCustomer 
            {
                ExternalId = emptyCustomer.ExternalId,
                Name = newCustomerName,
                IsDisabled = false,
                Shops = new ReadOnlyCollection<string>(new string [1]{Sitecore.Context.Site.Name})
            };

            customer.Users = new ReadOnlyCollection<string>(new string[1] { loggedInUserName });

            var request = new CreateCustomerRequest(customer);
            var result = cust.CreateCustomer(request);

            if (result.Success)
            {
                var parties = new List<Sitecore.Commerce.Entities.Party> { billToParty, shipToParty };
                var addPartiesRequest = new AddPartiesRequest(result.CommerceCustomer, parties);
                var addPartiesResult = cust.AddParties(addPartiesRequest);
                return result.CommerceCustomer;
            }

            throw new ApplicationException(result.SystemMessages.Any() ? result.SystemMessages[0].Message : Constants.Account.Error);

        }

        /// <summary>
        /// Get a customer's address
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Address> GetCustomerAddresses()
        {

            var addresses = new List<Address>();

            CommerceUser commerceUser = this.GetUser(Sitecore.Context.User.Name);

            if (commerceUser.UserName != null)
            {
 
                var customer = GetCustomer(commerceUser.ExternalId);

                var request = new GetPartiesRequest(customer);
                try
                {

                    var result = this._customerServiceProvider.GetParties(request);
                    if (request != null || result != null)
                    {
                        var partyList = result.Success && result.Parties != null ? (result.Parties).Cast<CommerceParty>() : new List<CommerceParty>();
                        if (partyList != null)
                        {
                            foreach (var party in partyList)
                            {

                                addresses.Add(new Address
                                {
                                    AddressName = party.Name,
                                    Id = party.ExternalId,
                                    FirstName = party.FirstName,
                                    LastName = party.LastName,
                                    Company = party.Company,
                                    Address1 = party.Address1,
                                    Address2 = party.Address2,
                                    City = party.City,
                                    State = party.State,
                                    Zip = party.ZipPostalCode,
                                    Country = party.Country,
                                    PartyId = party.PartyId,
                                    CountryCode = party.CountryCode,
                                    Phone = party.PhoneNumber,
                                    Fax = party.FaxNumber,
                                    Email = party.Email,
                                    IsMain = party.IsPrimary

                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message,this);
                }
            }

            return addresses;
        }

    }
}