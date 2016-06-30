
namespace CSDemo.Models
{

    using Sitecore.Commerce.Connect.CommerceServer;
    using Sitecore.Commerce.Connect.CommerceServer.Models;
    using Sitecore.Commerce.Connect.CommerceServer.Profiles.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Pipelines;
    using System.Collections.Generic;
    using CSP = CommerceServer.Core.Runtime.Profiles;

    /// <summary>
    /// A collection of useful profile extension methods
    /// </summary>
    public static class ProfileExtensions
    {
        /// <summary>
        /// Gets the commerce server profile that represents a user using their sitecore profile
        /// </summary>
        /// <param name="user">The sitecore user to get the commerce profile from</param>
        /// <returns>The associated commerce profile</returns>
        public static CSP.Profile GetCommerceProfile(this Sitecore.Security.Accounts.User user)
        {
            Sitecore.Security.UserProfile profile = user.Profile;

            var userId = profile.GetPropertyValue("user_id") as string;

            var commerceProfile = UserObject.GetCommerceProfile(userId);

            return commerceProfile;
        }

        /// <summary>
        /// Gets the commerce server profile as a model that represents a user using their sitecore profile
        /// </summary>
        /// <param name="user">The sitecore user to get the commerce profile from</param>
        /// <returns>The associated commerce profile as a model</returns>
        public static UserObject GetCommerceProfileModel(this Sitecore.Security.Accounts.User user)
        {
            Sitecore.Security.UserProfile profile = user.Profile;

            var userId = profile.GetPropertyValue("user_id") as string;

            var commerceProfileModel = UserObject.Get(userId);

            return commerceProfileModel;
        }

        /// <summary>
        /// Converts a list of commerce profiles into a list of commerce models
        /// </summary>
        /// <param name="profiles">The list of commerce profiles to convert</param>
        /// <returns>The converted profiles</returns>
        public static List<CommerceModel> ToCommerceModel(this List<CSP.Profile> profiles)
        {
            var cleanModels = new List<CommerceModel>();

            foreach (var profile in profiles)
            {
                var cleanModel = TranslateProfile<CommerceModel>(profile);

                cleanModels.Add(cleanModel);
            }

            return cleanModels;
        }

        /// <summary>
        /// Converts a list of commerce profiles into a list of commerce models
        /// </summary>
        /// <typeparam name="T">The type to convert the profiles to</typeparam>
        /// <param name="profiles">The list of commerce profiles to convert</param>
        /// <returns>The converted profiles</returns>
        public static List<T> ToCommerceModel<T>(this List<CSP.Profile> profiles)
            where T : Entity, new()
        {
            var cleanModels = new List<T>();

            if (profiles != null)
            {
                foreach (var profile in profiles)
                {
                    var cleanModel = TranslateProfile<T>(profile);

                    cleanModels.Add(cleanModel);
                }
            }

            return cleanModels;
        }

        /// <summary>
        /// Converts a profile to a commerce model
        /// </summary>
        /// <param name="profile">The profile to convert</param>
        /// <returns>The converted profile</returns>
        public static CommerceModel ToCommerceModel(this CSP.Profile profile)
        {
            return TranslateProfile<CommerceModel>(profile);
        }

        /// <summary>
        /// Converts a profile to a commerce model
        /// </summary>
        /// <typeparam name="T">The type to convert the profile to</typeparam>
        /// <param name="profile">The profile to convert</param>
        /// <returns>The converted profile</returns>
        public static T ToCommerceModel<T>(this CSP.Profile profile)
            where T : Entity, new()
        {
            return TranslateProfile<T>(profile);
        }

        /// <summary>
        /// Helper method to convert a profile to a commerce model
        /// </summary>
        /// <typeparam name="T">The type to convert the profile to</typeparam>
        /// <param name="profile">The profile to convert</param>
        /// <returns>The converted profile</returns>
        private static T TranslateProfile<T>(CSP.Profile profile) where T : Entity, new()
        {
            var args = new ProfileTranslatorArgs();
            args.InputParameters.Source = profile;

            CorePipeline.Run(CommerceConstants.PipelineNames.ProfileToCommerceModel, args);

            if (args.OutputParameters.Destination == null)
            {
                return default(T);
            }

            var newModel = new T();

            // newModel.Name = ((CommerceModel)args.OutputParameters.Destination).Name;
            newModel.Properties = ((Entity)args.OutputParameters.Destination).Properties;

            return newModel;
        }
    }

}