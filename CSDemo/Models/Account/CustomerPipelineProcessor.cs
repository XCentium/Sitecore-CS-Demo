using CommerceServer.Core.Runtime.Profiles;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Profiles.Pipelines;
using Sitecore.Commerce.Services;
using Sitecore.Pipelines;
using System;

namespace CSDemo.Models.Account
{
    public class CustomerPipelineProcessor : CommercePipelineProcessor
    {

        #region Protected methods
        /// <summary>
        /// Gets the commerce address profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="profile">The profile.</param>
        /// <returns>The service provider result.</returns>
        protected ServiceProviderResult GetCommerceAddressProfile(string id, ref Profile profile)
        {
            return this.GetCommerceProfile(id, "Address", ref profile);
        }

        /// <summary>
        /// Gets the commerce user profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="profile">The profile.</param>
        /// <returns>The service provider result.</returns>
        protected ServiceProviderResult GetCommerceUserProfile(string id, ref Profile profile)
        {
            return this.GetCommerceProfile(id, "UserObject", ref profile);
        }

        /// <summary>
        /// Creates the address profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="profile">The profile.</param>
        /// <returns>The service provider result.</returns>
        protected ServiceProviderResult CreateAddressProfile(string id, ref Profile profile)
        {
            ServiceProviderResult result = new ServiceProviderResult { Success = true };

            try
            {
                var createArgs = new CreateProfileArgs();
                createArgs.InputParameters.Name = "Address";
                createArgs.InputParameters.Id = id;

                CorePipeline.Run(CommerceConstants.PipelineNames.CreateProfile, createArgs);
                profile = createArgs.OutputParameters.CommerceProfile;
            }
            catch (Exception e)
            {
                result = new ServiceProviderResult { Success = false };
                result.SystemMessages.Add(new SystemMessage { Message = e.Message });
            }

            return result;
        }

        /// <summary>
        /// Deletes the address commerce profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The service provider result.</returns>
        protected ServiceProviderResult DeleteAddressCommerceProfile(string id)
        {
            return this.DeleteCommerceProfile(id, "Address");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the commerce profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="profileName">Name of the profile.</param>
        /// <param name="profile">The profile.</param>
        /// <returns>The service provider result.</returns>
        private ServiceProviderResult GetCommerceProfile(string id, string profileName, ref Profile profile)
        {
            ServiceProviderResult result = new ServiceProviderResult { Success = true };

            try
            {
                var createArgs = new GetProfileArgs();
                createArgs.InputParameters.Name = profileName;
                createArgs.InputParameters.Id = id;

                CorePipeline.Run(CommerceConstants.PipelineNames.GetProfile, createArgs);
                profile = createArgs.OutputParameters.CommerceProfile;
            }
            catch (Exception e)
            {
                result = new ServiceProviderResult { Success = false };
                result.SystemMessages.Add(new SystemMessage { Message = e.Message });
            }

            return result;
        }

        /// <summary>
        /// Deletes the commerce profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>The service provider result.</returns>
        private ServiceProviderResult DeleteCommerceProfile(string id, string profileName)
        {
            ServiceProviderResult result = new ServiceProviderResult { Success = true };

            try
            {
                var deleteArgs = new DeleteProfileArgs();
                deleteArgs.InputParameters.Name = profileName;
                deleteArgs.InputParameters.Id = id;

                CorePipeline.Run(CommerceConstants.PipelineNames.DeleteProfile, deleteArgs);
                result.Success = deleteArgs.OutputParameters.Success;
            }
            catch (Exception e)
            {
                result = new ServiceProviderResult { Success = false };
                result.SystemMessages.Add(new SystemMessage { Message = e.Message });
            }

            return result;
        }

        #endregion
    }
}