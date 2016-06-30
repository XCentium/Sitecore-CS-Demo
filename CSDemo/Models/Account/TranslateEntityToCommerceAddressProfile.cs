﻿using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Diagnostics;

namespace CSDemo.Models.Account
{



    /// <summary>
    /// Defines the TranslateEntityToCommerceAddressProfile class.
    /// </summary>
    public class TranslateEntityToCommerceAddressProfile : CommerceTranslateProcessor
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Sitecore.Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Request, "args.request");
            Assert.ArgumentNotNull(args.Result, "args.result"); // TranslateCommerceAddressProfileToEntityRequest
            Assert.ArgumentCondition(args.Request is TranslateEntityToCommerceAddressProfileRequest, "args.Request ", "args.Request is TranslateEntityToCommerceAddressProfileRequest");

            var request = (TranslateEntityToCommerceAddressProfileRequest)args.Request;
            Assert.ArgumentNotNull(request.SourceParty, "request.SourceParty");
            Assert.ArgumentNotNull(request.DestinationProfile, "request.DestinationProfile");

            if (request.SourceParty is CommerceParty)
            {
                this.TranslateCommerceCustomerParty(request.SourceParty as CommerceParty, request.DestinationProfile);
            }
            else
            {
                this.TranslateCustomParty(request.SourceParty, request.DestinationProfile);
            }
        }

        /// <summary>
        /// Translates the commerce customer party.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="profile">The profile.</param>
        protected virtual void TranslateCommerceCustomerParty(CommerceParty party, CommerceServer.Core.Runtime.Profiles.Profile profile)
        {
            profile["GeneralInfo.first_name"].Value = party.FirstName;
            profile["GeneralInfo.last_name"].Value = party.LastName;
            profile["GeneralInfo.address_name"].Value = party.Name;
            profile["GeneralInfo.address_line1"].Value = party.Address1;
            profile["GeneralInfo.address_line2"].Value = party.Address2;
            profile["GeneralInfo.city"].Value = party.City;
            profile["GeneralInfo.region_code"].Value = party.RegionCode;
            profile["GeneralInfo.region_name"].Value = party.RegionName;
            profile["GeneralInfo.postal_code"].Value = party.ZipPostalCode;
            profile["GeneralInfo.country_code"].Value = party.CountryCode;
            profile["GeneralInfo.country_name"].Value = party.Country;
            profile["GeneralInfo.tel_number"].Value = party.PhoneNumber;
            profile["GeneralInfo.region_code"].Value = party.State;

            this.TranslateCommerceCustomerPartyCustomProperties(party, profile);
        }

        /// <summary>
        /// Translates the commerce customer party custom properties.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="profile">The profile.</param>
        protected virtual void TranslateCommerceCustomerPartyCustomProperties(CommerceParty party, CommerceServer.Core.Runtime.Profiles.Profile profile)
        {
        }

        /// <summary>
        /// Translates the custom party.
        /// </summary>
        /// <param name="party">The party.</param>
        /// <param name="profile">The profile.</param>
        private void TranslateCustomParty(CommerceParty party, CommerceServer.Core.Runtime.Profiles.Profile profile)
        {
        }
    }
}