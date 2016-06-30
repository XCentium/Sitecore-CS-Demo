using CommerceServer.Core.Runtime.Profiles;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Models;
using Sitecore.Commerce.Connect.CommerceServer.Profiles.Models;
using Sitecore.Commerce.Connect.CommerceServer.Profiles.Pipelines;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSDemo.Models
{

    /// <summary>
    /// A representation of the ProfileBase CommerceEntity in the Metadata system.
    /// </summary>


    public partial class ProfileBase : CommerceModel
    {



    }

    /// <summary>
    /// A representation of the Address CommerceEntity in the Metadata system.
    /// </summary>


    public partial class Address : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the AddressID property.
        /// </summary>
        /// <value>
        /// The AddressID property from the property collection.
        /// </value>
        public virtual string AddressID
        {
            get { return this.GetPropertyValue(PropertyNames.AddressID) as string; }
            set { this.SetPropertyValue(PropertyNames.AddressID, value); }
        }

        /// <summary>
        /// Gets or sets the LastName property.
        /// </summary>
        /// <value>
        /// The LastName property from the property collection.
        /// </value>
        public virtual string LastName
        {
            get { return this.GetPropertyValue(PropertyNames.LastName) as string; }
            set { this.SetPropertyValue(PropertyNames.LastName, value); }
        }

        /// <summary>
        /// Gets or sets the FirstName property.
        /// </summary>
        /// <value>
        /// The FirstName property from the property collection.
        /// </value>
        public virtual string FirstName
        {
            get { return this.GetPropertyValue(PropertyNames.FirstName) as string; }
            set { this.SetPropertyValue(PropertyNames.FirstName, value); }
        }

        /// <summary>
        /// Gets or sets the AddressName property.
        /// </summary>
        /// <value>
        /// The AddressName property from the property collection.
        /// </value>
        public virtual string AddressName
        {
            get { return this.GetPropertyValue(PropertyNames.AddressName) as string; }
            set { this.SetPropertyValue(PropertyNames.AddressName, value); }
        }

        /// <summary>
        /// Gets or sets the AddressType property.
        /// </summary>
        /// <value>
        /// The AddressType property from the property collection.
        /// </value>
        public virtual int? AddressType
        {
            get { return this.GetPropertyValue(PropertyNames.AddressType) as int?; }
            set { this.SetPropertyValue(PropertyNames.AddressType, value); }
        }

        /// <summary>
        /// Gets or sets the Description property.
        /// </summary>
        /// <value>
        /// The Description property from the property collection.
        /// </value>
        public virtual string Description
        {
            get { return this.GetPropertyValue(PropertyNames.Description) as string; }
            set { this.SetPropertyValue(PropertyNames.Description, value); }
        }

        /// <summary>
        /// Gets or sets the AddressLine1 property.
        /// </summary>
        /// <value>
        /// The AddressLine1 property from the property collection.
        /// </value>
        public virtual string AddressLine1
        {
            get { return this.GetPropertyValue(PropertyNames.AddressLine1) as string; }
            set { this.SetPropertyValue(PropertyNames.AddressLine1, value); }
        }

        /// <summary>
        /// Gets or sets the AddressLine2 property.
        /// </summary>
        /// <value>
        /// The AddressLine2 property from the property collection.
        /// </value>
        public virtual string AddressLine2
        {
            get { return this.GetPropertyValue(PropertyNames.AddressLine2) as string; }
            set { this.SetPropertyValue(PropertyNames.AddressLine2, value); }
        }

        /// <summary>
        /// Gets or sets the City property.
        /// </summary>
        /// <value>
        /// The City property from the property collection.
        /// </value>
        public virtual string City
        {
            get { return this.GetPropertyValue(PropertyNames.City) as string; }
            set { this.SetPropertyValue(PropertyNames.City, value); }
        }

        /// <summary>
        /// Gets or sets the StateProvinceCode property.
        /// </summary>
        /// <value>
        /// The StateProvinceCode property from the property collection.
        /// </value>
        public virtual string StateProvinceCode
        {
            get { return this.GetPropertyValue(PropertyNames.StateProvinceCode) as string; }
            set { this.SetPropertyValue(PropertyNames.StateProvinceCode, value); }
        }

        /// <summary>
        /// Gets or sets the StateProvinceName property.
        /// </summary>
        /// <value>
        /// The StateProvinceName property from the property collection.
        /// </value>
        public virtual string StateProvinceName
        {
            get { return this.GetPropertyValue(PropertyNames.StateProvinceName) as string; }
            set { this.SetPropertyValue(PropertyNames.StateProvinceName, value); }
        }

        /// <summary>
        /// Gets or sets the ZipPostalCode property.
        /// </summary>
        /// <value>
        /// The ZipPostalCode property from the property collection.
        /// </value>
        public virtual string ZipPostalCode
        {
            get { return this.GetPropertyValue(PropertyNames.ZipPostalCode) as string; }
            set { this.SetPropertyValue(PropertyNames.ZipPostalCode, value); }
        }

        /// <summary>
        /// Gets or sets the CountryRegionCode property.
        /// </summary>
        /// <value>
        /// The CountryRegionCode property from the property collection.
        /// </value>
        public virtual string CountryRegionCode
        {
            get { return this.GetPropertyValue(PropertyNames.CountryRegionCode) as string; }
            set { this.SetPropertyValue(PropertyNames.CountryRegionCode, value); }
        }

        /// <summary>
        /// Gets or sets the CountryRegionName property.
        /// </summary>
        /// <value>
        /// The CountryRegionName property from the property collection.
        /// </value>
        public virtual string CountryRegionName
        {
            get { return this.GetPropertyValue(PropertyNames.CountryRegionName) as string; }
            set { this.SetPropertyValue(PropertyNames.CountryRegionName, value); }
        }

        /// <summary>
        /// Gets or sets the TelephoneNumber property.
        /// </summary>
        /// <value>
        /// The TelephoneNumber property from the property collection.
        /// </value>
        public virtual string TelephoneNumber
        {
            get { return this.GetPropertyValue(PropertyNames.TelephoneNumber) as string; }
            set { this.SetPropertyValue(PropertyNames.TelephoneNumber, value); }
        }

        /// <summary>
        /// Gets or sets the TelephoneExtension property.
        /// </summary>
        /// <value>
        /// The TelephoneExtension property from the property collection.
        /// </value>
        public virtual string TelephoneExtension
        {
            get { return this.GetPropertyValue(PropertyNames.TelephoneExtension) as string; }
            set { this.SetPropertyValue(PropertyNames.TelephoneExtension, value); }
        }

        /// <summary>
        /// Gets or sets the LocaleID property.
        /// </summary>
        /// <value>
        /// The LocaleID property from the property collection.
        /// </value>
        public virtual int? LocaleID
        {
            get { return this.GetPropertyValue(PropertyNames.LocaleID) as int?; }
            set { this.SetPropertyValue(PropertyNames.LocaleID, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the Address profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.address_id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the Address CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string AddressID = "GeneralInfo.address_id";
            public static string LastName = "GeneralInfo.last_name";
            public static string FirstName = "GeneralInfo.first_name";
            public static string AddressName = "GeneralInfo.address_name";
            public static string AddressType = "GeneralInfo.address_type";
            public static string Description = "GeneralInfo.description";
            public static string AddressLine1 = "GeneralInfo.address_line1";
            public static string AddressLine2 = "GeneralInfo.address_line2";
            public static string City = "GeneralInfo.city";
            public static string StateProvinceCode = "GeneralInfo.region_code";
            public static string StateProvinceName = "GeneralInfo.region_name";
            public static string ZipPostalCode = "GeneralInfo.postal_code";
            public static string CountryRegionCode = "GeneralInfo.country_code";
            public static string CountryRegionName = "GeneralInfo.country_name";
            public static string TelephoneNumber = "GeneralInfo.tel_number";
            public static string TelephoneExtension = "GeneralInfo.tel_extension";
            public static string LocaleID = "GeneralInfo.locale";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(Address.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Create a Address profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a Address profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return Address.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a Address profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "Address";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a Address profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created Address model</returns>
        public static Address CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Address>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the Address profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static Address Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Address>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<Address> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Address";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<Address>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the BlanketPOs CommerceEntity in the Metadata system.
    /// </summary>


    public partial class BlanketPOs : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the PurchaseOrderID property.
        /// </summary>
        /// <value>
        /// The PurchaseOrderID property from the property collection.
        /// </value>
        public virtual string PurchaseOrderID
        {
            get { return this.GetPropertyValue(PropertyNames.PurchaseOrderID) as string; }
            set { this.SetPropertyValue(PropertyNames.PurchaseOrderID, value); }
        }

        /// <summary>
        /// Gets or sets the OrganizationID property.
        /// </summary>
        /// <value>
        /// The OrganizationID property from the property collection.
        /// </value>
        public virtual string OrganizationID
        {
            get { return this.GetPropertyValue(PropertyNames.OrganizationID) as string; }
            set { this.SetPropertyValue(PropertyNames.OrganizationID, value); }
        }

        /// <summary>
        /// Gets or sets the PONumber property.
        /// </summary>
        /// <value>
        /// The PONumber property from the property collection.
        /// </value>
        public virtual string PONumber
        {
            get { return this.GetPropertyValue(PropertyNames.PONumber) as string; }
            set { this.SetPropertyValue(PropertyNames.PONumber, value); }
        }

        /// <summary>
        /// Gets or sets the Description property.
        /// </summary>
        /// <value>
        /// The Description property from the property collection.
        /// </value>
        public virtual string Description
        {
            get { return this.GetPropertyValue(PropertyNames.Description) as string; }
            set { this.SetPropertyValue(PropertyNames.Description, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the BlanketPOs profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.po_id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the BlanketPOs CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string PurchaseOrderID = "GeneralInfo.po_id";
            public static string OrganizationID = "GeneralInfo.org_id";
            public static string PONumber = "GeneralInfo.po_number";
            public static string Description = "GeneralInfo.description";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(BlanketPOs.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Create a BlanketPOs profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a BlanketPOs profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return BlanketPOs.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a BlanketPOs profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "BlanketPOs";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a BlanketPOs profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created BlanketPOs model</returns>
        public static BlanketPOs CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<BlanketPOs>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the BlanketPOs profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static BlanketPOs Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<BlanketPOs>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<BlanketPOs> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "BlanketPOs";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<BlanketPOs>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the CreditCard CommerceEntity in the Metadata system.
    /// </summary>


    public partial class CreditCard : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the CreditCardID property.
        /// </summary>
        /// <value>
        /// The CreditCardID property from the property collection.
        /// </value>
        public virtual string CreditCardID
        {
            get { return this.GetPropertyValue(PropertyNames.CreditCardID) as string; }
            set { this.SetPropertyValue(PropertyNames.CreditCardID, value); }
        }

        /// <summary>
        /// Gets or sets the PaymentGroupID property.
        /// </summary>
        /// <value>
        /// The PaymentGroupID property from the property collection.
        /// </value>
        public virtual string PaymentGroupID
        {
            get { return this.GetPropertyValue(PropertyNames.PaymentGroupID) as string; }
            set { this.SetPropertyValue(PropertyNames.PaymentGroupID, value); }
        }

        /// <summary>
        /// Gets or sets the AccountNumber property.
        /// </summary>
        /// <value>
        /// The AccountNumber property from the property collection.
        /// </value>
        public virtual string AccountNumber
        {
            get { return this.GetPropertyValue(PropertyNames.AccountNumber) as string; }
            set { this.SetPropertyValue(PropertyNames.AccountNumber, value); }
        }

        /// <summary>
        /// Gets or sets the LastFourDigits property.
        /// </summary>
        /// <value>
        /// The LastFourDigits property from the property collection.
        /// </value>
        public virtual string LastFourDigits
        {
            get { return this.GetPropertyValue(PropertyNames.LastFourDigits) as string; }
            set { this.SetPropertyValue(PropertyNames.LastFourDigits, value); }
        }

        /// <summary>
        /// Gets or sets the BillingAddress property.
        /// </summary>
        /// <value>
        /// The BillingAddress property from the property collection.
        /// </value>
        public virtual string BillingAddress
        {
            get { return this.GetPropertyValue(PropertyNames.BillingAddress) as string; }
            set { this.SetPropertyValue(PropertyNames.BillingAddress, value); }
        }

        /// <summary>
        /// Gets or sets the ExpirationMonth property.
        /// </summary>
        /// <value>
        /// The ExpirationMonth property from the property collection.
        /// </value>
        public virtual int? ExpirationMonth
        {
            get { return this.GetPropertyValue(PropertyNames.ExpirationMonth) as int?; }
            set { this.SetPropertyValue(PropertyNames.ExpirationMonth, value); }
        }

        /// <summary>
        /// Gets or sets the ExpirationYear property.
        /// </summary>
        /// <value>
        /// The ExpirationYear property from the property collection.
        /// </value>
        public virtual int? ExpirationYear
        {
            get { return this.GetPropertyValue(PropertyNames.ExpirationYear) as int?; }
            set { this.SetPropertyValue(PropertyNames.ExpirationYear, value); }
        }

        /// <summary>
        /// Gets or sets the KeyIndex property.
        /// </summary>
        /// <value>
        /// The KeyIndex property from the property collection.
        /// </value>
        public virtual int? KeyIndex
        {
            get { return this.GetPropertyValue(PropertyNames.KeyIndex) as int?; }
            set { this.SetPropertyValue(PropertyNames.KeyIndex, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the CreditCard profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the CreditCard CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string CreditCardID = "GeneralInfo.id";
            public static string PaymentGroupID = "GeneralInfo.payment_group_id";
            public static string AccountNumber = "GeneralInfo.cc_number";
            public static string LastFourDigits = "GeneralInfo.last_4_digits";
            public static string BillingAddress = "GeneralInfo.billing_address";
            public static string ExpirationMonth = "GeneralInfo.expiration_month";
            public static string ExpirationYear = "GeneralInfo.expiration_year";
            public static string KeyIndex = "ProfileSystem.KeyIndex";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(CreditCard.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Create a CreditCard profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a CreditCard profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return CreditCard.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a CreditCard profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "CreditCard";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a CreditCard profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created CreditCard model</returns>
        public static CreditCard CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<CreditCard>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the CreditCard profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static CreditCard Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<CreditCard>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<CreditCard> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "CreditCard";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<CreditCard>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the Currency CommerceEntity in the Metadata system.
    /// </summary>


    public partial class Currency : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the CurrencyCode property.
        /// </summary>
        /// <value>
        /// The CurrencyCode property from the property collection.
        /// </value>
        public virtual string CurrencyCode
        {
            get { return this.GetPropertyValue(PropertyNames.CurrencyCode) as string; }
            set { this.SetPropertyValue(PropertyNames.CurrencyCode, value); }
        }

        /// <summary>
        /// Gets or sets the CurrencyCulture property.
        /// </summary>
        /// <value>
        /// The CurrencyCulture property from the property collection.
        /// </value>
        public virtual string CurrencyCulture
        {
            get { return this.GetPropertyValue(PropertyNames.CurrencyCulture) as string; }
            set { this.SetPropertyValue(PropertyNames.CurrencyCulture, value); }
        }

        /// <summary>
        /// Gets or sets the ConversionFactor property.
        /// </summary>
        /// <value>
        /// The ConversionFactor property from the property collection.
        /// </value>
        public virtual float? ConversionFactor
        {
            get { return this.GetPropertyValue(PropertyNames.ConversionFactor) as float?; }
            set { this.SetPropertyValue(PropertyNames.ConversionFactor, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the Currency profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.currency_code";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the Currency CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string CurrencyCode = "GeneralInfo.currency_code";
            public static string CurrencyCulture = "GeneralInfo.currency_culture";
            public static string ConversionFactor = "GeneralInfo.conversion_factor";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(Currency.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Create a Currency profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a Currency profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return Currency.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a Currency profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "Currency";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a Currency profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created Currency model</returns>
        public static Currency CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Currency>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the Currency profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static Currency Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Currency>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<Currency> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Currency";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<Currency>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the Organization CommerceEntity in the Metadata system.
    /// </summary>


    public partial class Organization : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the OrganizationID property.
        /// </summary>
        /// <value>
        /// The OrganizationID property from the property collection.
        /// </value>
        public virtual string OrganizationID
        {
            get { return this.GetPropertyValue(PropertyNames.OrganizationID) as string; }
            set { this.SetPropertyValue(PropertyNames.OrganizationID, value); }
        }

        /// <summary>
        /// Gets or sets the Name property.
        /// </summary>
        /// <value>
        /// The Name property from the property collection.
        /// </value>
        public virtual string Name
        {
            get { return this.GetPropertyValue(PropertyNames.Name) as string; }
            set { this.SetPropertyValue(PropertyNames.Name, value); }
        }

        /// <summary>
        /// Gets or sets the TradingPartnerNumber property.
        /// </summary>
        /// <value>
        /// The TradingPartnerNumber property from the property collection.
        /// </value>
        public virtual string TradingPartnerNumber
        {
            get { return this.GetPropertyValue(PropertyNames.TradingPartnerNumber) as string; }
            set { this.SetPropertyValue(PropertyNames.TradingPartnerNumber, value); }
        }

        /// <summary>
        /// Gets or sets the AdministrativeContact property.
        /// </summary>
        /// <value>
        /// The AdministrativeContact property from the property collection.
        /// </value>
        public virtual string AdministrativeContact
        {
            get { return this.GetPropertyValue(PropertyNames.AdministrativeContact) as string; }
            set { this.SetPropertyValue(PropertyNames.AdministrativeContact, value); }
        }

        /// <summary>
        /// Gets or sets the Receiver property.
        /// </summary>
        /// <value>
        /// The Receiver property from the property collection.
        /// </value>
        public virtual string Receiver
        {
            get { return this.GetPropertyValue(PropertyNames.Receiver) as string; }
            set { this.SetPropertyValue(PropertyNames.Receiver, value); }
        }

        /// <summary>
        /// Gets or sets the PreferredAddress property.
        /// </summary>
        /// <value>
        /// The PreferredAddress property from the property collection.
        /// </value>
        public virtual string PreferredAddress
        {
            get { return this.GetPropertyValue(PropertyNames.PreferredAddress) as string; }
            set { this.SetPropertyValue(PropertyNames.PreferredAddress, value); }
        }

        /// <summary>
        /// Gets or sets the AddressList property.
        /// </summary>
        /// <value>
        /// The AddressList property from the property collection.
        /// </value>
        private ProfilePropertyListCollection<string> _AddressList;
        public virtual ProfilePropertyListCollection<string> AddressList
        {
            get
            {
                if (_AddressList != null)
                {
                    return _AddressList;
                }

                var profileValue = this.GetPropertyValue(PropertyNames.AddressList) as object[];

                if (profileValue != null)
                {
                    var e = profileValue.Select(i => i.ToString());
                    _AddressList = new ProfilePropertyListCollection<string>(e);
                    _AddressList.ClearDirtyFlag();
                }
                else
                {
                    _AddressList = new ProfilePropertyListCollection<string>();
                }

                return _AddressList;
            }
            set
            {
                _AddressList = null;

                if (value == null || value.Count == 0)
                {
                    this.SetPropertyValue(PropertyNames.AddressList, DBNull.Value);
                }
                else
                {
                    this.SetPropertyValue(PropertyNames.AddressList, value.Cast<object>().ToArray());
                }
            }
        }
        /// <summary>
        /// Gets or sets the OrganizationCatalogSet property.
        /// </summary>
        /// <value>
        /// The OrganizationCatalogSet property from the property collection.
        /// </value>
        public virtual string OrganizationCatalogSet
        {
            get { return this.GetPropertyValue(PropertyNames.OrganizationCatalogSet) as string; }
            set { this.SetPropertyValue(PropertyNames.OrganizationCatalogSet, value); }
        }

        /// <summary>
        /// Gets or sets the PurchasingGroup property.
        /// </summary>
        /// <value>
        /// The PurchasingGroup property from the property collection.
        /// </value>
        public virtual string PurchasingGroup
        {
            get { return this.GetPropertyValue(PropertyNames.PurchasingGroup) as string; }
            set { this.SetPropertyValue(PropertyNames.PurchasingGroup, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the Organization profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.org_id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the Organization CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string OrganizationID = "GeneralInfo.org_id";
            public static string Name = "GeneralInfo.name";
            public static string TradingPartnerNumber = "GeneralInfo.trading_partner_number";
            public static string AdministrativeContact = "GeneralInfo.user_id_admin_contact";
            public static string Receiver = "GeneralInfo.user_id_receiver";
            public static string PreferredAddress = "GeneralInfo.preferred_address";
            public static string AddressList = "GeneralInfo.address_list";
            public static string OrganizationCatalogSet = "GeneralInfo.org_catalog_set";
            public static string PurchasingGroup = "GeneralInfo.purchasing";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }
            this.CheckPropertyLists();

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(Organization.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Checks and updates property lists to ensure changes get pushed back to the profile system
        /// </summary>
        public void CheckPropertyLists()
        {
            if (AddressList.IsDirty)
            {
                AddressList = AddressList;
            }

        }

        /// <summary>
        /// Create a Organization profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a Organization profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return Organization.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a Organization profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "Organization";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a Organization profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created Organization model</returns>
        public static Organization CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Organization>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the Organization profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static Organization Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<Organization>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<Organization> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "Organization";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<Organization>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the TargetingContext CommerceEntity in the Metadata system.
    /// </summary>


    public partial class TargetingContext : ProfileBase
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the UniqueID property.
        /// </summary>
        /// <value>
        /// The UniqueID property from the property collection.
        /// </value>
        public virtual string UniqueID
        {
            get { return this.GetPropertyValue(PropertyNames.UniqueID) as string; }
            set { this.SetPropertyValue(PropertyNames.UniqueID, value); }
        }

        /// <summary>
        /// Gets or sets the PageGroup property.
        /// </summary>
        /// <value>
        /// The PageGroup property from the property collection.
        /// </value>
        public virtual string PageGroup
        {
            get { return this.GetPropertyValue(PropertyNames.PageGroup) as string; }
            set { this.SetPropertyValue(PropertyNames.PageGroup, value); }
        }

        /// <summary>
        /// Gets or sets the Channel property.
        /// </summary>
        /// <value>
        /// The Channel property from the property collection.
        /// </value>
        public virtual string Channel
        {
            get { return this.GetPropertyValue(PropertyNames.Channel) as string; }
            set { this.SetPropertyValue(PropertyNames.Channel, value); }
        }

        /// <summary>
        /// Gets or sets the UserLocale property.
        /// </summary>
        /// <value>
        /// The UserLocale property from the property collection.
        /// </value>
        public virtual string UserLocale
        {
            get { return this.GetPropertyValue(PropertyNames.UserLocale) as string; }
            set { this.SetPropertyValue(PropertyNames.UserLocale, value); }
        }

        /// <summary>
        /// Gets or sets the UserUILocale property.
        /// </summary>
        /// <value>
        /// The UserUILocale property from the property collection.
        /// </value>
        public virtual string UserUILocale
        {
            get { return this.GetPropertyValue(PropertyNames.UserUILocale) as string; }
            set { this.SetPropertyValue(PropertyNames.UserUILocale, value); }
        }

        /// <summary>
        /// The primary key of the TargetingContext profile type.
        /// </summary>
        protected const string _primaryKey = "unique_id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the TargetingContext CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string UniqueID = "unique_id";
            public static string PageGroup = "pagegroup";
            public static string Channel = "channel";
            public static string UserLocale = "user_locale";
            public static string UserUILocale = "user_ui_locale";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(TargetingContext.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Create a TargetingContext profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a TargetingContext profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return TargetingContext.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a TargetingContext profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "TargetingContext";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a TargetingContext profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created TargetingContext model</returns>
        public static TargetingContext CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<TargetingContext>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the TargetingContext profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static TargetingContext Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<TargetingContext>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<TargetingContext> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "TargetingContext";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<TargetingContext>();

            return cleanModels;
        }
    }

    /// <summary>
    /// A representation of the UserObject CommerceEntity in the Metadata system.
    /// </summary>


    public partial class UserObject : CommerceCustomer
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the UserID property.
        /// </summary>
        /// <value>
        /// The UserID property from the property collection.
        /// </value>
        public virtual string UserID
        {
            get { return this.GetPropertyValue(PropertyNames.UserID) as string; }
            set { this.SetPropertyValue(PropertyNames.UserID, value); }
        }

        /// <summary>
        /// Gets or sets the ExternalID property.
        /// </summary>
        /// <value>
        /// The ExternalID property from the property collection.
        /// </value>
        public override string ExternalId
        {
            get { return this.GetPropertyValue(PropertyNames.ExternalID) as string; }
            set { this.SetPropertyValue(PropertyNames.ExternalID, value); }
        }

        /// <summary>
        /// Gets or sets the UserPassword property.
        /// </summary>
        /// <value>
        /// The UserPassword property from the property collection.
        /// </value>
        public virtual string UserPassword
        {
            get { return this.GetPropertyValue(PropertyNames.UserPassword) as string; }
            set { this.SetPropertyValue(PropertyNames.UserPassword, value); }
        }

        /// <summary>
        /// Gets or sets the Email property.
        /// </summary>
        /// <value>
        /// The Email property from the property collection.
        /// </value>
        public virtual string Email
        {
            get { return this.GetPropertyValue(PropertyNames.Email) as string; }
            set { this.SetPropertyValue(PropertyNames.Email, value); }
        }

        /// <summary>
        /// Gets or sets the PreferredAddress property.
        /// </summary>
        /// <value>
        /// The PreferredAddress property from the property collection.
        /// </value>
        public virtual string PreferredAddress
        {
            get { return this.GetPropertyValue(PropertyNames.PreferredAddress) as string; }
            set { this.SetPropertyValue(PropertyNames.PreferredAddress, value); }
        }

        /// <summary>
        /// Gets or sets the AddressList property.
        /// </summary>
        /// <value>
        /// The AddressList property from the property collection.
        /// </value>
        private ProfilePropertyListCollection<string> _AddressList;
        public virtual ProfilePropertyListCollection<string> AddressList
        {
            get
            {
                if (_AddressList != null)
                {
                    return _AddressList;
                }

                var profileValue = this.GetPropertyValue(PropertyNames.AddressList) as object[];

                if (profileValue != null)
                {
                    var e = profileValue.Select(i => i.ToString());
                    _AddressList = new ProfilePropertyListCollection<string>(e);
                    _AddressList.ClearDirtyFlag();
                }
                else
                {
                    _AddressList = new ProfilePropertyListCollection<string>();
                }

                return _AddressList;
            }
            set
            {
                _AddressList = null;

                if (value == null || value.Count == 0)
                {
                    this.SetPropertyValue(PropertyNames.AddressList, DBNull.Value);
                }
                else
                {
                    this.SetPropertyValue(PropertyNames.AddressList, value.Cast<object>().ToArray());
                }
            }
        }
        /// <summary>
        /// Gets or sets the PreferredCreditCard property.
        /// </summary>
        /// <value>
        /// The PreferredCreditCard property from the property collection.
        /// </value>
        public virtual string PreferredCreditCard
        {
            get { return this.GetPropertyValue(PropertyNames.PreferredCreditCard) as string; }
            set { this.SetPropertyValue(PropertyNames.PreferredCreditCard, value); }
        }

        /// <summary>
        /// Gets or sets the CreditCardList property.
        /// </summary>
        /// <value>
        /// The CreditCardList property from the property collection.
        /// </value>
        private ProfilePropertyListCollection<string> _CreditCardList;
        public virtual ProfilePropertyListCollection<string> CreditCardList
        {
            get
            {
                if (_CreditCardList != null)
                {
                    return _CreditCardList;
                }

                var profileValue = this.GetPropertyValue(PropertyNames.CreditCardList) as object[];

                if (profileValue != null)
                {
                    var e = profileValue.Select(i => i.ToString());
                    _CreditCardList = new ProfilePropertyListCollection<string>(e);
                    _CreditCardList.ClearDirtyFlag();
                }
                else
                {
                    _CreditCardList = new ProfilePropertyListCollection<string>();
                }

                return _CreditCardList;
            }
            set
            {
                _CreditCardList = null;

                if (value == null || value.Count == 0)
                {
                    this.SetPropertyValue(PropertyNames.CreditCardList, DBNull.Value);
                }
                else
                {
                    this.SetPropertyValue(PropertyNames.CreditCardList, value.Cast<object>().ToArray());
                }
            }
        }
        /// <summary>
        /// Gets or sets the UserType property.
        /// </summary>
        /// <value>
        /// The UserType property from the property collection.
        /// </value>
        public virtual string UserType
        {
            get { return this.GetPropertyValue(PropertyNames.UserType) as string; }
            set { this.SetPropertyValue(PropertyNames.UserType, value); }
        }

        /// <summary>
        /// Gets or sets the LastName property.
        /// </summary>
        /// <value>
        /// The LastName property from the property collection.
        /// </value>
        public virtual string LastName
        {
            get { return this.GetPropertyValue(PropertyNames.LastName) as string; }
            set { this.SetPropertyValue(PropertyNames.LastName, value); }
        }

        /// <summary>
        /// Gets or sets the FirstName property.
        /// </summary>
        /// <value>
        /// The FirstName property from the property collection.
        /// </value>
        public virtual string FirstName
        {
            get { return this.GetPropertyValue(PropertyNames.FirstName) as string; }
            set { this.SetPropertyValue(PropertyNames.FirstName, value); }
        }

        /// <summary>
        /// Gets or sets the TelephoneNumber property.
        /// </summary>
        /// <value>
        /// The TelephoneNumber property from the property collection.
        /// </value>
        public virtual string TelephoneNumber
        {
            get { return this.GetPropertyValue(PropertyNames.TelephoneNumber) as string; }
            set { this.SetPropertyValue(PropertyNames.TelephoneNumber, value); }
        }

        /// <summary>
        /// Gets or sets the TelephoneExtension property.
        /// </summary>
        /// <value>
        /// The TelephoneExtension property from the property collection.
        /// </value>
        public virtual string TelephoneExtension
        {
            get { return this.GetPropertyValue(PropertyNames.TelephoneExtension) as string; }
            set { this.SetPropertyValue(PropertyNames.TelephoneExtension, value); }
        }

        /// <summary>
        /// Gets or sets the FaxNumber property.
        /// </summary>
        /// <value>
        /// The FaxNumber property from the property collection.
        /// </value>
        public virtual string FaxNumber
        {
            get { return this.GetPropertyValue(PropertyNames.FaxNumber) as string; }
            set { this.SetPropertyValue(PropertyNames.FaxNumber, value); }
        }

        /// <summary>
        /// Gets or sets the FaxExtension property.
        /// </summary>
        /// <value>
        /// The FaxExtension property from the property collection.
        /// </value>
        public virtual string FaxExtension
        {
            get { return this.GetPropertyValue(PropertyNames.FaxExtension) as string; }
            set { this.SetPropertyValue(PropertyNames.FaxExtension, value); }
        }

        /// <summary>
        /// Gets or sets the DefaultLanguage property.
        /// </summary>
        /// <value>
        /// The DefaultLanguage property from the property collection.
        /// </value>
        public virtual string DefaultLanguage
        {
            get { return this.GetPropertyValue(PropertyNames.DefaultLanguage) as string; }
            set { this.SetPropertyValue(PropertyNames.DefaultLanguage, value); }
        }

        /// <summary>
        /// Gets or sets the PasswordQuestion property.
        /// </summary>
        /// <value>
        /// The PasswordQuestion property from the property collection.
        /// </value>
        public virtual string PasswordQuestion
        {
            get { return this.GetPropertyValue(PropertyNames.PasswordQuestion) as string; }
            set { this.SetPropertyValue(PropertyNames.PasswordQuestion, value); }
        }

        /// <summary>
        /// Gets or sets the PasswordAnswer property.
        /// </summary>
        /// <value>
        /// The PasswordAnswer property from the property collection.
        /// </value>
        public virtual string PasswordAnswer
        {
            get { return this.GetPropertyValue(PropertyNames.PasswordAnswer) as string; }
            set { this.SetPropertyValue(PropertyNames.PasswordAnswer, value); }
        }

        /// <summary>
        /// Gets or sets the DirectMailOptOut property.
        /// </summary>
        /// <value>
        /// The DirectMailOptOut property from the property collection.
        /// </value>
        public virtual bool? DirectMailOptOut
        {
            get { return this.GetPropertyValue(PropertyNames.DirectMailOptOut) as bool?; }
            set { this.SetPropertyValue(PropertyNames.DirectMailOptOut, value); }
        }

        /// <summary>
        /// Gets or sets the ExpressCheckout property.
        /// </summary>
        /// <value>
        /// The ExpressCheckout property from the property collection.
        /// </value>
        public virtual bool? ExpressCheckout
        {
            get { return this.GetPropertyValue(PropertyNames.ExpressCheckout) as bool?; }
            set { this.SetPropertyValue(PropertyNames.ExpressCheckout, value); }
        }

        /// <summary>
        /// Gets or sets the PreferredShippingMethod property.
        /// </summary>
        /// <value>
        /// The PreferredShippingMethod property from the property collection.
        /// </value>
        public virtual string PreferredShippingMethod
        {
            get { return this.GetPropertyValue(PropertyNames.PreferredShippingMethod) as string; }
            set { this.SetPropertyValue(PropertyNames.PreferredShippingMethod, value); }
        }

        /// <summary>
        /// Gets or sets the DefaultShoppingList property.
        /// </summary>
        /// <value>
        /// The DefaultShoppingList property from the property collection.
        /// </value>
        public virtual string DefaultShoppingList
        {
            get { return this.GetPropertyValue(PropertyNames.DefaultShoppingList) as string; }
            set { this.SetPropertyValue(PropertyNames.DefaultShoppingList, value); }
        }

        /// <summary>
        /// Gets or sets the Comment property.
        /// </summary>
        /// <value>
        /// The Comment property from the property collection.
        /// </value>
        public virtual string Comment
        {
            get { return this.GetPropertyValue(PropertyNames.Comment) as string; }
            set { this.SetPropertyValue(PropertyNames.Comment, value); }
        }

        /// <summary>
        /// Gets or sets the OrganizationID property.
        /// </summary>
        /// <value>
        /// The OrganizationID property from the property collection.
        /// </value>
        public virtual string OrganizationID
        {
            get { return this.GetPropertyValue(PropertyNames.OrganizationID) as string; }
            set { this.SetPropertyValue(PropertyNames.OrganizationID, value); }
        }

        /// <summary>
        /// Gets or sets the AccountStatus property.
        /// </summary>
        /// <value>
        /// The AccountStatus property from the property collection.
        /// </value>
        public virtual string AccountStatus
        {
            get { return this.GetPropertyValue(PropertyNames.AccountStatus) as string; }
            set { this.SetPropertyValue(PropertyNames.AccountStatus, value); }
        }

        /// <summary>
        /// Gets or sets the UserCatalogSet property.
        /// </summary>
        /// <value>
        /// The UserCatalogSet property from the property collection.
        /// </value>
        public virtual string UserCatalogSet
        {
            get { return this.GetPropertyValue(PropertyNames.UserCatalogSet) as string; }
            set { this.SetPropertyValue(PropertyNames.UserCatalogSet, value); }
        }

        /// <summary>
        /// Gets or sets the DateRegistered property.
        /// </summary>
        /// <value>
        /// The DateRegistered property from the property collection.
        /// </value>
        public virtual DateTime? DateRegistered
        {
            get { return this.GetPropertyValue(PropertyNames.DateRegistered) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateRegistered, value); }
        }

        /// <summary>
        /// Gets or sets the AccessLevel property.
        /// </summary>
        /// <value>
        /// The AccessLevel property from the property collection.
        /// </value>
        public virtual int? AccessLevel
        {
            get { return this.GetPropertyValue(PropertyNames.AccessLevel) as int?; }
            set { this.SetPropertyValue(PropertyNames.AccessLevel, value); }
        }

        /// <summary>
        /// Gets or sets the CampaignHistory property.
        /// </summary>
        /// <value>
        /// The CampaignHistory property from the property collection.
        /// </value>
        public virtual string CampaignHistory
        {
            get { return this.GetPropertyValue(PropertyNames.CampaignHistory) as string; }
            set { this.SetPropertyValue(PropertyNames.CampaignHistory, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastChanged property.
        /// </summary>
        /// <value>
        /// The DateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreated property.
        /// </summary>
        /// <value>
        /// The DateCreated property from the property collection.
        /// </value>
        public virtual DateTime? DateCreated
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreated) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the ChangedBy property.
        /// </summary>
        /// <value>
        /// The ChangedBy property from the property collection.
        /// </value>
        public virtual string ChangedBy
        {
            get { return this.GetPropertyValue(PropertyNames.ChangedBy) as string; }
            set { this.SetPropertyValue(PropertyNames.ChangedBy, value); }
        }

        /// <summary>
        /// Gets or sets the KeyIndex property.
        /// </summary>
        /// <value>
        /// The KeyIndex property from the property collection.
        /// </value>
        public virtual int? KeyIndex
        {
            get { return this.GetPropertyValue(PropertyNames.KeyIndex) as int?; }
            set { this.SetPropertyValue(PropertyNames.KeyIndex, value); }
        }

        /// <summary>
        /// Gets or sets the DatePasswordLastChanged property.
        /// </summary>
        /// <value>
        /// The DatePasswordLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DatePasswordLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DatePasswordLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DatePasswordLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the ChangePassword property.
        /// </summary>
        /// <value>
        /// The ChangePassword property from the property collection.
        /// </value>
        public virtual bool? ChangePassword
        {
            get { return this.GetPropertyValue(PropertyNames.ChangePassword) as bool?; }
            set { this.SetPropertyValue(PropertyNames.ChangePassword, value); }
        }

        /// <summary>
        /// Gets or sets the DateLastLogon property.
        /// </summary>
        /// <value>
        /// The DateLastLogon property from the property collection.
        /// </value>
        public virtual DateTime? DateLastLogon
        {
            get { return this.GetPropertyValue(PropertyNames.DateLastLogon) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateLastLogon, value); }
        }

        /// <summary>
        /// Gets or sets the LogonErrorDates property.
        /// </summary>
        /// <value>
        /// The LogonErrorDates property from the property collection.
        /// </value>
        private ProfilePropertyListCollection<string> _LogonErrorDates;
        public virtual ProfilePropertyListCollection<string> LogonErrorDates
        {
            get
            {
                if (_LogonErrorDates != null)
                {
                    return _LogonErrorDates;
                }

                var profileValue = this.GetPropertyValue(PropertyNames.LogonErrorDates) as string;

                if (profileValue != null)
                {
                    var e = profileValue.Select(i => i.ToString());
                    _LogonErrorDates = new ProfilePropertyListCollection<string>(e);
                    _LogonErrorDates.ClearDirtyFlag();
                }
                else
                {
                    _LogonErrorDates = new ProfilePropertyListCollection<string>();
                }

                return _LogonErrorDates;
            }
            set
            {
                _LogonErrorDates = null;

                if (value == null || value.Count == 0)
                {
                    this.SetPropertyValue(PropertyNames.LogonErrorDates, DBNull.Value);
                }
                else
                {
                    this.SetPropertyValue(PropertyNames.LogonErrorDates, value.Cast<object>().ToArray());
                }
            }
        }
        /// <summary>
        /// Gets or sets the PasswordAnswerErrorDates property.
        /// </summary>
        /// <value>
        /// The PasswordAnswerErrorDates property from the property collection.
        /// </value>
        private ProfilePropertyListCollection<string> _PasswordAnswerErrorDates;
        public virtual ProfilePropertyListCollection<string> PasswordAnswerErrorDates
        {
            get
            {
                if (_PasswordAnswerErrorDates != null)
                {
                    return _PasswordAnswerErrorDates;
                }

                var profileValue = this.GetPropertyValue(PropertyNames.PasswordAnswerErrorDates) as string;

                if (profileValue != null)
                {
                    var e = profileValue.Select(i => i.ToString());
                    _PasswordAnswerErrorDates = new ProfilePropertyListCollection<string>(e);
                    _PasswordAnswerErrorDates.ClearDirtyFlag();
                }
                else
                {
                    _PasswordAnswerErrorDates = new ProfilePropertyListCollection<string>();
                }

                return _PasswordAnswerErrorDates;
            }
            set
            {
                _PasswordAnswerErrorDates = null;

                if (value == null || value.Count == 0)
                {
                    this.SetPropertyValue(PropertyNames.PasswordAnswerErrorDates, DBNull.Value);
                }
                else
                {
                    this.SetPropertyValue(PropertyNames.PasswordAnswerErrorDates, value.Cast<object>().ToArray());
                }
            }
        }
        /// <summary>
        /// Gets or sets the AdapterDateLastChanged property.
        /// </summary>
        /// <value>
        /// The AdapterDateLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? AdapterDateLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.AdapterDateLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.AdapterDateLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the ApplicationName property.
        /// </summary>
        /// <value>
        /// The ApplicationName property from the property collection.
        /// </value>
        public virtual string ApplicationName
        {
            get { return this.GetPropertyValue(PropertyNames.ApplicationName) as string; }
            set { this.SetPropertyValue(PropertyNames.ApplicationName, value); }
        }

        /// <summary>
        /// Gets or sets the LastActivityDate property.
        /// </summary>
        /// <value>
        /// The LastActivityDate property from the property collection.
        /// </value>
        public virtual DateTime? LastActivityDate
        {
            get { return this.GetPropertyValue(PropertyNames.LastActivityDate) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.LastActivityDate, value); }
        }

        /// <summary>
        /// Gets or sets the LastLockedOutDate property.
        /// </summary>
        /// <value>
        /// The LastLockedOutDate property from the property collection.
        /// </value>
        public virtual DateTime? LastLockedOutDate
        {
            get { return this.GetPropertyValue(PropertyNames.LastLockedOutDate) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.LastLockedOutDate, value); }
        }

        /// <summary>
        /// Gets or sets the DateAddressListLastChanged property.
        /// </summary>
        /// <value>
        /// The DateAddressListLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateAddressListLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateAddressListLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateAddressListLastChanged, value); }
        }

        /// <summary>
        /// Gets or sets the DateCreditCardListLastChanged property.
        /// </summary>
        /// <value>
        /// The DateCreditCardListLastChanged property from the property collection.
        /// </value>
        public virtual DateTime? DateCreditCardListLastChanged
        {
            get { return this.GetPropertyValue(PropertyNames.DateCreditCardListLastChanged) as DateTime?; }
            set { this.SetPropertyValue(PropertyNames.DateCreditCardListLastChanged, value); }
        }

        /// <summary>
        /// The primary key of the UserObject profile type.
        /// </summary>
        protected const string _primaryKey = "GeneralInfo.user_id";

        /// <summary>
        /// Gets the primary key for this profile type.
        /// </summary>
        public virtual string PrimaryKey
        {
            get { return _primaryKey; }
        }


        #endregion



        /// <summary>
        /// Properties of the UserObject CommerceEntity in the Metadata system.
        /// </summary>
        public class PropertyNames : CommerceModel.PropertyName
        {
            public static string UserID = "GeneralInfo.user_id";
            public static string ExternalID = "GeneralInfo.ExternalId";
            public static string UserPassword = "GeneralInfo.user_security_password";
            public static string Email = "GeneralInfo.email_address";
            public static string PreferredAddress = "GeneralInfo.preferred_address";
            public static string AddressList = "GeneralInfo.address_list";
            public static string PreferredCreditCard = "GeneralInfo.preferred_credit_card";
            public static string CreditCardList = "GeneralInfo.credit_card_list";
            public static string UserType = "GeneralInfo.user_type";
            public static string LastName = "GeneralInfo.last_name";
            public static string FirstName = "GeneralInfo.first_name";
            public static string TelephoneNumber = "GeneralInfo.tel_number";
            public static string TelephoneExtension = "GeneralInfo.tel_extension";
            public static string FaxNumber = "GeneralInfo.fax_number";
            public static string FaxExtension = "GeneralInfo.fax_extension";
            public static string DefaultLanguage = "GeneralInfo.language";
            public static string PasswordQuestion = "GeneralInfo.password_question";
            public static string PasswordAnswer = "GeneralInfo.password_answer";
            public static string DirectMailOptOut = "GeneralInfo.direct_mail_opt_out";
            public static string ExpressCheckout = "GeneralInfo.express_checkout";
            public static string PreferredShippingMethod = "GeneralInfo.preferred_shipping_method";
            public static string DefaultShoppingList = "GeneralInfo.default_shopper_list";
            public static string Comment = "GeneralInfo.comment";
            public static string OrganizationID = "AccountInfo.org_id";
            public static string AccountStatus = "AccountInfo.account_status";
            public static string UserCatalogSet = "AccountInfo.user_catalog_set";
            public static string DateRegistered = "AccountInfo.date_registered";
            public static string AccessLevel = "AccountInfo.access_level";
            public static string CampaignHistory = "Advertising.campaign_history";
            public static string DateLastChanged = "ProfileSystem.date_last_changed";
            public static string DateCreated = "ProfileSystem.date_created";
            public static string ChangedBy = "ProfileSystem.user_id_changed_by";
            public static string KeyIndex = "ProfileSystem.KeyIndex";
            public static string DatePasswordLastChanged = "ProfileSystem.date_last_password_changed";
            public static string ChangePassword = "ProfileSystem.change_password";
            public static string DateLastLogon = "ProfileSystem.date_last_logon";
            public static string LogonErrorDates = "ProfileSystem.logon_error_dates";
            public static string PasswordAnswerErrorDates = "ProfileSystem.password_answer_error_dates";
            public static string AdapterDateLastChanged = "ProfileSystem.csadapter_date_last_changed";
            public static string ApplicationName = "ProfileSystem.application_name";
            public static string LastActivityDate = "ProfileSystem.last_activity_date";
            public static string LastLockedOutDate = "ProfileSystem.last_lockedout_date";
            public static string DateAddressListLastChanged = "ProfileSystem.date_address_list_last_changed";
            public static string DateCreditCardListLastChanged = "ProfileSystem.date_credit_card_list_last_changed";

        }

        /// <summary>
        /// Save the provided profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        public void Save(Profile profile)
        {
            if (profile == null)
            {
                return;
            }

            bool emailUpdated = false;
            this.CheckPropertyLists();

            foreach (var property in this.Properties)
            {
                if ((property.Key != CommerceModel.PropertyName.Id) && (PrimaryKey != property.Key))
                {
                    if (property.Key == PropertyNames.Email)
                    {
                        emailUpdated = true;
                    }

                    if (property.Value != null)
                    {
                        profile.Properties[property.Key].Value = property.Value;
                    }
                    else
                    {
                        profile.Properties[property.Key].Value = System.DBNull.Value;
                    }
                }
            }

            this.SetPropertyValue(PrimaryKey, profile.Properties[PrimaryKey].Value);

            profile.Update();
            var user = Sitecore.Context.User;
            if (user != null && user.IsAuthenticated)
            {
                if (emailUpdated)
                {
                    user.Profile.Email = this.Email;
                    user.Profile.Save();
                }

                Sitecore.Caching.CacheManager.ClearUserProfileCache(user.Name);
            }
        }

        /// <summary>
        /// Save the current profile
        /// </summary>
        public void Save()
        {
            string primaryVal = this.GetPropertyValue(PrimaryKey) as string;
            if (!string.IsNullOrEmpty(primaryVal))
            {
                this.Save(UserObject.GetCommerceProfile(primaryVal));
            }
        }

        /// <summary>
        /// Checks and updates property lists to ensure changes get pushed back to the profile system
        /// </summary>
        public void CheckPropertyLists()
        {
            if (AddressList.IsDirty)
            {
                AddressList = AddressList;
            }

            if (CreditCardList.IsDirty)
            {
                CreditCardList = CreditCardList;
            }

            if (LogonErrorDates.IsDirty)
            {
                LogonErrorDates = LogonErrorDates;
            }

            if (PasswordAnswerErrorDates.IsDirty)
            {
                PasswordAnswerErrorDates = PasswordAnswerErrorDates;
            }

        }

        /// <summary>
        /// Create a UserObject profile type
        /// </summary>
        /// <param name="id">unique id for the profile</param>
        /// <returns>newly created profile</returns>
        public static Profile Create(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Create a UserObject profile type
        /// </summary>
        /// <returns>newly created profile</returns>
        public static Profile Create()
        {
            return UserObject.Create(Guid.NewGuid().ToString("B"));
        }

        /// <summary>
        /// Deletes a UserObject profile
        /// </summary>
        /// <param name="id">ID of the profile to delete</param>
        /// <returns>True if the profile was deleted</returns>
        public static bool Delete(string id)
        {
            var args = new DeleteProfileArgs();
            args.InputParameters.Name = "UserObject";
            args.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, args);
            var success = args.OutputParameters.Success;

            return success;
        }

        /// <summary>
        /// Creates a UserObject profile as a clean model
        /// </summary>
        /// <param name="id">A unique id for the profile</param>
        /// <returns>The newly created UserObject model</returns>
        public static UserObject CreateAsModel(string id)
        {
            var createArgs = new CreateProfileArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<UserObject>();

            return cleanModel;
        }

        /// <summary>
        /// Get a Commerce Profile object based on the provided id
        /// </summary>
        /// <param name="id">id of the UserObject profile to retreive</param>
        /// <returns>The found profile or null if not found</returns>
        public static Profile GetCommerceProfile(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;

            return profile;
        }

        /// <summary>
        /// Get a CommerceModel based on the provided id
        /// </summary>
        /// <param name="id">id of the profile to retreive</param>
        /// <returns>The model based on the profile that was found, or empty if not found.</returns>
        public static UserObject Get(string id)
        {
            var createArgs = new GetProfileArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Id = id;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
            var profile = createArgs.OutputParameters.CommerceProfile;
            var cleanModel = profile.ToCommerceModel<UserObject>();

            return cleanModel;
        }

        /// <summary>
        /// Get a list of Commerce profiles
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>A list of profiles that was found</returns>
        public static List<Profile> GetCommerceProfiles(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;

            return profiles;
        }

        /// <summary>
        /// Gets a list of models based on the provided profile ids
        /// </summary>
        /// <param name="ids">list of ids of the profiles to retreive</param>
        /// <returns>The list of models based on the found profiles</returns>
        public static List<UserObject> GetMultiple(List<string> ids)
        {
            var createArgs = new GetProfilesArgs();
            createArgs.InputParameters.Name = "UserObject";
            createArgs.InputParameters.Ids = ids;

            CorePipeline.Run(CommerceConstants.PipelineNames.GetProfiles, createArgs);
            var profiles = createArgs.OutputParameters.CommerceProfiles;
            var cleanModels = profiles.ToCommerceModel<UserObject>();

            return cleanModels;
        }
    }

}