#region
		
using System;
using System.Web.Security;
using Ninject;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using Sitecore.Social.Connector.Pipelines.MatchUser;
using Sitecore.Social.Infrastructure;
using Sitecore.Social.Infrastructure.Logging;
 
	#endregion
namespace CSDemo.Configuration.Social
{
    /// <summary>
    ///     A SelectConnectorUser pipeline processor. The class is responsible create a new user for connector if it wasn't
    ///     found in previous processors
    /// </summary>
    public class CreateUser
    {
        /// <summary>
        ///     Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Process(SelectUserPipelineArgs args)
        {
            Assert.IsNotNull(args, "The pipeline args is null");
            Assert.IsNotNull(args.CreateNewUserIfNoExistingFound,
                "The CreateNewUserIfNoExistingFound property in pipeline args must be filled");
            Assert.IsNotNull(args.Username, "The Username property in pipeline args must be filled");
            Assert.IsNotNull(args.Email, "The Email property in pipeline args must be filled");
            if (args.Result != null || !args.CreateNewUserIfNoExistingFound)
            {
                return;
            }
            var fullName = Context.Domain.GetFullName(args.Username);
           
            var splitter = fullName.Split('\\');
            var commerceUserFullName = $"{Constants.Commerce.DefaultSocialDomainForCommerce}\\{splitter[1]}";
            args.Result = CreateSitecoreUser(commerceUserFullName, args.Email, args.AccountBasicData.FullName);
        }

        /// <summary>
        ///     Create a new Sitecore extranet user.
        /// </summary>
        /// <param name="domainUser">
        ///     Domain user name.
        /// </param>
        /// <param name="email">
        ///     User email.
        /// </param>
        /// <param name="fullName">
        ///     Full user name.
        /// </param>
        /// <returns>
        ///     The <see cref="T:Sitecore.Security.Accounts.User" />.
        /// </returns>
        private User CreateSitecoreUser(string domainUser, string email, string fullName)
        {
            var length = 128;
            var numberOfNonAlphanumericCharacters = 20;
            if (Membership.GetUser(domainUser) != null)
            {
                var membershipUser1 = Membership.GetUser(domainUser);
                if (membershipUser1 != null)
                    return User.FromName(membershipUser1.UserName, false);
            }
            var membershipUser = Membership.CreateUser(domainUser,
                Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters), email);
            membershipUser.IsApproved = true;
            Membership.UpdateUser(membershipUser);
            var user = User.FromName(membershipUser.UserName, false);
            try
            {
                string profileItemId;
                using (new SecurityDisabler())
                {
                    profileItemId =
                        Client.CoreDatabase.GetItem("/sitecore/system/Settings/Security/Profiles/Commerce User").ID.ToString();
                }
                user.Profile.Initialize(user.Name, true);
                user.Profile.ProfileItemId = profileItemId;
                user.Profile.FullName = fullName;
                user.Profile.Email = email;
                user.Profile.Save();
            }
            catch (Exception ex)
            {
                ExecutingContext.Current.IoC.Get<ILogManager>().LogMessage(ex.Message, LogLevel.Error, this, ex);
            }
            Event.RaiseEvent("social:connector:user:created", user);
            return user;
        }
    }
}