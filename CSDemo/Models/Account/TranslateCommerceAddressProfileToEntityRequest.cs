using Sitecore;

namespace CSDemo.Models.Account
{
    using CommerceServer.Core.Runtime.Profiles;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Defines the TranslateCommerceAddressProfileToEntityRequest class.
    /// </summary>
    public class TranslateCommerceAddressProfileToEntityRequest : CommerceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateCommerceAddressProfileToEntityRequest"/> class.
        /// </summary>
        /// <param name="sourceProfile">The source profile.</param>
        /// <param name="destinationParty">The destination party.</param>
        public TranslateCommerceAddressProfileToEntityRequest([NotNull] Profile sourceProfile, [NotNull] CommerceParty destinationParty)
        {
            Assert.ArgumentNotNull(sourceProfile, "commerceProfile");
            Assert.ArgumentNotNull(destinationParty, "customerParty");

            this.SourceProfile = sourceProfile;
            this.DestinationParty = destinationParty;
        }

        /// <summary>
        /// Gets or sets the source profile.
        /// </summary>
        /// <value>
        /// The source profile.
        /// </value>
        public Profile SourceProfile { get; set; }

        /// <summary>
        /// Gets or sets the destination party.
        /// </summary>
        /// <value>
        /// The destination party.
        /// </value>
        public CommerceParty DestinationParty { get; set; }
    }
}