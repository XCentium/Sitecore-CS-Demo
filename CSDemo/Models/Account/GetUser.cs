﻿using Sitecore.Commerce.Data.Customers;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Pipelines.Customers.GetUser;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Diagnostics;
using Sitecore.Security;
using Sitecore.Security.Accounts;
using System.Collections.Generic;

namespace CSDemo.Models.Account
{
    /// <summary>
    /// Extends the get user pipeline to add some additional data to the CommerceUser
    /// </summary>
    public class GetUser : GetUserFromSitecore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUser"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public GetUser(IUserRepository userRepository)
            : base(userRepository)
        {
        }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(ServicePipelineArgs args)
        {
            var request = (GetUserRequest)args.Request;
            var result = (GetUserResult)args.Result;

            base.Process(args);

            if (result.CommerceUser == null)
            {
                return;
            }

            // if we found a user, add some addition info
            var userProfile = GetUserProfile(result.CommerceUser.UserName);
            Assert.IsNotNull(userProfile, "profile");

            UpdateCustomer(result.CommerceUser, userProfile);
        }

        /// <summary>
        /// Updates the customer with some additional data.
        /// </summary>
        /// <param name="commerceUser">The commerce user to update.</param>
        /// <param name="userProfile">The user profile of the user.</param>
        protected void UpdateCustomer(Sitecore.Commerce.Entities.Customers.CommerceUser commerceUser, Sitecore.Security.UserProfile userProfile)
        {
            commerceUser.ExternalId = userProfile["user_id"];
            Assert.IsNotNullOrEmpty(commerceUser.ExternalId, "commerceUser.ExternalId");

            if (commerceUser.Customers == null || commerceUser.Customers.Count == 0)
            {
                var customers = new List<string>() { commerceUser.ExternalId };
                commerceUser.Customers = customers.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the user profile from the external system.
        /// </summary>
        /// <param name="userName">The username of the profile to retrieve.</param>
        /// <returns>The profile of the user</returns>
        protected virtual UserProfile GetUserProfile(string userName)
        {
            return User.FromName(userName, true).Profile;
        }
    }
}