#region

using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore.Analytics;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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
            CartHelper = cartHelper;
            _customerServiceProvider = customerServiceProvider;
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
            var anonymousUserId = CartHelper.GetVisitorId();
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
            Cookie.Set(Constants.Commerce.CommerceUserLoggedIn, Constants.Common.True);
            var catalogOptions = GetLoggedInCustomerCatalogOptions();

            if (string.IsNullOrEmpty(catalogOptions)) return;

            Cookie.Set(Constants.Commerce.UserCatalogOptions, catalogOptions);

            // if more than one set the cookie options
            if (catalogOptions.Contains(Constants.Common.PipeStringSeparator))
            {
                Cookie.Set(Constants.Commerce.UserSelectedCatalogId, "");
                Cookie.Set(Constants.Commerce.UserSelectedCatalogPostfix, "");
            }
            else
            {
                // if it is just one catalog, get the ID and set the cookie
                SetUserCatalogChoice(catalogOptions);
            }
        }


        internal void SetUserCatalogChoice(string catalogName)
        {
            // if it is just one catalog, get the ID and set the cookie
            var catalogItem = ProductHelper.GetCatalogItemByName(catalogName);

            if (catalogItem == null) return;

            Cookie.Set(Constants.Commerce.UserSelectedCatalogId, catalogItem.ID.ToString());

            if (!catalogItem.HasChildren) return;
            
            // get the first child and check if if has a postfix starting with (
            foreach (Item child in catalogItem.Children)
            {
                var firstChild = child.Name;
                if (firstChild.Contains("("))
                {
                    var postFix = firstChild.Substring(firstChild.IndexOf("(", StringComparison.Ordinal));
                    Cookie.Set(Constants.Commerce.UserSelectedCatalogPostfix, postFix);
                }
                break;
            }
        }

        /// <summary>
        /// Update a user profile in Sitecore
        /// </summary>
        internal void UpdateContactProfile()
        {
            if (Sitecore.Context.User.IsAuthenticated)
            {

                Tracker.Current.Session.Identify(Sitecore.Context.User.Name);

                if (!string.IsNullOrEmpty(GetCommerceUserId(Sitecore.Context.User.Name)))
                {
                    var user = Sitecore.Context.User;
                    var profile = user.Profile;
                    var userName = user.LocalName;
                    var customer = GetUser(Sitecore.Context.User.Name);

                    if (customer.UserName != null)
                    {
                        if (Tracker.Current.Contact != null)
                        {
                            // Update email information Faucet
                            var emailFacet = Tracker.Current.Contact.GetFacet<IContactEmailAddresses>(Constants.Account.FacetEmail);

                            //Check if an work email address already exists for the contact
                            if (!emailFacet.Entries.Contains(Constants.Account.PersonalEmail))
                            {
                                var email = emailFacet.Entries.Create(Constants.Account.PersonalEmail);
                                email.SmtpAddress = customer.Email.Trim();
                            }
                            else
                            {
                                var email = emailFacet.Entries[Constants.Account.PersonalEmail];
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
                                    profile.FullName = $"{customer.FirstName} {customer.LastName}".Trim();
                                }

                                profile.Save();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(profile.FullName.Trim()) && profile.FullName.Trim().Contains(" "))
                                {
                                    var names = profile.FullName.Trim().Split(' ');
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
                            var photoPath = Constants.Account.CustomerPhotoPath + user.LocalName;
                            MediaItem photoItem = Sitecore.Context.Database.GetItem(photoPath);

                            if (photoItem == null) return;

                            var stream = photoItem.GetMediaStream();
                            var memoryStream = new MemoryStream();

                            if (stream == null) return;

                            stream.CopyTo(memoryStream);
                            pictureFacet.Picture = memoryStream.ToArray();
                            pictureFacet.MimeType = photoItem.MimeType;
                        }
                    }

                }

            }

        }

        internal void UpdateContactProfile(User user)
        {
            if (user == null) return;
            Tracker.Current.Session.Identify(user.Name);

            if (!string.IsNullOrEmpty(GetCommerceUserId(user.Name)))
            {

                var profile = user.Profile;
                var userName = user.LocalName;
                var customer = GetUser(user.Name);

                if (customer.UserName != null)
                {

                    if (Tracker.Current.Contact != null)
                    {
                        // Update email information Faucet
                        var emailFacet = Tracker.Current.Contact.GetFacet<IContactEmailAddresses>(Constants.Account.FacetEmail);

                        //Check if an work email address already exists for the contact
                        if (!emailFacet.Entries.Contains(Constants.Account.PersonalEmail))
                        {
                            var email = emailFacet.Entries.Create(Constants.Account.PersonalEmail);
                            email.SmtpAddress = customer.Email.Trim();

                        }
                        else
                        {
                            var email = emailFacet.Entries[Constants.Account.PersonalEmail];
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
                                var names = profile.FullName.Trim().Split(' ');
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
                        var photoPath = Constants.Account.CustomerPhotoPath + user.LocalName;
                        MediaItem photoItem = Sitecore.Context.Database.GetItem(photoPath);

                        if (photoItem != null)
                        {
                            var stream = photoItem.GetMediaStream();
                            var memoryStream = new MemoryStream();
                            if (stream != null)
                            {
                                stream.CopyTo(memoryStream);
                                pictureFacet.Picture = memoryStream.ToArray();
                                pictureFacet.MimeType = photoItem.MimeType;
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
            var result = _customerServiceProvider.GetUser(request);
            UpdateUserCustomerInfo(userName, result.CommerceUser);

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

            if (string.IsNullOrEmpty(commerceUser.ExternalId) ||
                (commerceUser.Customers != null && commerceUser.Customers.Count != 0)) return commerceUser;

            var customers = new List<string>() { commerceUser.ExternalId };
            commerceUser.Customers = customers.AsReadOnly();

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
            var profile = user.Profile;
            var commerceUserId = user.Profile.GetCustomProperty(Constants.Commerce.CommerceCustomerId);

            if (!string.IsNullOrEmpty(commerceUserId)) return commerceUserId;

            commerceUserId = profile[Constants.Commerce.CommerceUserId];

            if (string.IsNullOrEmpty(commerceUserId)) return commerceUserId;

            // Create custom profile for scommerce_customer_id
            // reload the profile
            using (new UserSwitcher(user))
            {
                user.Profile.SetCustomProperty(Constants.Commerce.CommerceCustomerId, commerceUserId);
                user.Profile.Save();
                user.Profile.Reload();
            }

            return commerceUserId;
        }

        internal string GetLoggedInCustomerCatalogOptions()
        {
            if (!Sitecore.Context.User.IsAuthenticated) return string.Empty;

            var userName = Sitecore.Context.User.Name;
            var user = User.FromName(userName, true);
            var profile = user.Profile;
            var commerceCatalogSet = profile[Constants.Commerce.CommerceUserCatalogSetId];

            //commerceCatalogSet = "{22222222-2222-2222-2222-222222222222}";
            if (!string.IsNullOrEmpty(commerceCatalogSet) && commerceCatalogSet.Contains(Constants.Common.Dash))
            {
                return GetCatalogNamesFromCatalogSet(commerceCatalogSet);
            }

            return string.Empty;
        }


        private string GetCatalogNamesFromCatalogSet(string commerceCatalogSet)
        {
            if (!string.IsNullOrEmpty(commerceCatalogSet))
            {
                var commerceCatalogSetItem = Sitecore.Context.Database.GetItem(commerceCatalogSet);

                if (commerceCatalogSetItem == null) return string.Empty;

                var commerceCatalogNames = commerceCatalogSetItem[Constants.Account.CatalogSetField];
                if (!string.IsNullOrEmpty(commerceCatalogNames))
                {
                    return commerceCatalogNames;
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
                        var catalogNames = commerceCatalogNames.Split(Constants.Common.PipeSeparator);

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
            var result = _customerServiceProvider.GetCustomer(request);

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

        public void ClearUserCatalogCookies()
        {
            Cookie.Del(Constants.Commerce.CommerceUserLoggedIn);
            Cookie.Del(Constants.Commerce.UserCatalogOptions);
            Cookie.Del(Constants.Commerce.UserSelectedCatalogId);
            Cookie.Del(Constants.Commerce.UserSelectedCatalogPostfix);
            Cookie.Del(Constants.Commerce.ShowCoupon);
            Cookie.Del(Constants.Commerce.CouponMessage);
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
                userName = matches[0].Name;
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
            var commerceUser = GetUser(Sitecore.Context.User.Name);

            if (commerceUser.UserName != null)
            {
                User.FromName(Sitecore.Context.User.Name, true);

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
                    var newCustomerName =
                        $"{customerAddress.Company.Trim()}.{customerAddress.LastName.Trim()}.{customerAddress.FirstName.Trim()}.{created}";

                    CreateCustomer(customer, newCustomerName, party, party, Sitecore.Context.User.LocalName);
                }
                catch (Exception)
                {
                    return false;
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
                Shops = new ReadOnlyCollection<string>(new string[1] {Sitecore.Context.Site.Name}),
                Users = new ReadOnlyCollection<string>(new string[1] {loggedInUserName})
            };


            var request = new CreateCustomerRequest(customer);
            var result = cust.CreateCustomer(request);

            if (result.Success)
            {
                var parties = new List<Sitecore.Commerce.Entities.Party> { billToParty, shipToParty };
                var addPartiesRequest = new AddPartiesRequest(result.CommerceCustomer, parties);
                cust.AddParties(addPartiesRequest);
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
            var commerceUser = GetUser(Sitecore.Context.User.Name);

            if (commerceUser.UserName != null)
            {

                var customer = GetCustomer(commerceUser.ExternalId);

                var request = new GetPartiesRequest(customer);
                try
                {
                    var result = _customerServiceProvider.GetParties(request);
                    {
                        var partyList = result.Success && result.Parties != null ? (result.Parties).Cast<CommerceParty>() : new List<CommerceParty>();

                        addresses.AddRange(partyList.Select(party => new Address
                        {
                            AddressName = party.Name, Id = party.ExternalId, FirstName = party.FirstName, LastName = party.LastName, Company = party.Company, Address1 = party.Address1, Address2 = party.Address2, City = party.City, State = party.State, Zip = party.ZipPostalCode, Country = party.Country, PartyId = party.PartyId, CountryCode = party.CountryCode, Phone = party.PhoneNumber, Fax = party.FaxNumber, Email = party.Email, IsMain = party.IsPrimary
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error(ex.Message, this);
                }
            }

            return addresses;
        }


        public static List<Models.Address> GetUserAddresses()
        {
            var user = Sitecore.Context.User;
            var commerceProfile = user.GetCommerceProfileModel();

            if (commerceProfile?.AddressList != null)
            {
                return Models.Address.GetMultiple(commerceProfile.AddressList.InnerList());
            }

            return new List<Models.Address>();  //{ new CSDemo.Models.Address() };
        }

        internal string GetCurrentCustomerCatalogPostFix()
        {
            return Cookie.Get(Constants.Commerce.UserSelectedCatalogPostfix) != null ? Cookie.Get(Constants.Commerce.UserSelectedCatalogPostfix).Value : null;
        }
    }
}