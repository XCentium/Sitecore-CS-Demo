using Sitecore;

namespace CSDemo.Models.Account
{
    using CommerceServer.Core.Runtime.Profiles;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Defines the TranslateEntityToCommerceAddressProfileRequest class.
    /// </summary>
    public class TranslateEntityToCommerceAddressProfileRequest : CommerceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateEntityToCommerceAddressProfileRequest"/> class.
        /// </summary>
        /// <param name="sourceParty">The source party.</param>
        /// <param name="destinationProfile">The destination profile.</param>
        public TranslateEntityToCommerceAddressProfileRequest([NotNull] CommerceParty sourceParty, [NotNull] Profile destinationProfile)
        {
            Assert.ArgumentNotNull(destinationProfile, "commerceProfile");
            Assert.ArgumentNotNull(sourceParty, "customerParty");

            this.DestinationProfile = destinationProfile;
            this.SourceParty = sourceParty;
        }

        /// <summary>
        /// Gets or sets the destination profile.
        /// </summary>
        /// <value>
        /// The destination profile.
        /// </value>
        public Profile DestinationProfile { get; set; }

        /// <summary>
        /// Gets or sets the source party.
        /// </summary>
        /// <value>
        /// The source party.
        /// </value>
        public CommerceParty SourceParty { get; set; }
    }
}