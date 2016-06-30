using CommerceServer.Core.Runtime.Profiles;
using Sitecore;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Pipelines;
using Sitecore.Commerce.Connect.CommerceServer.Profiles.Models;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace CSDemo.Models.Account
{
    /// <summary>
    /// Defines the GetParties class.
    /// </summary>
    public class GetParties : CustomerPipelineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetParties"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        public GetParties([NotNull] IEntityFactory entityFactory)
        {
            Assert.ArgumentNotNull(entityFactory, "entityFactory");

            this.EntityFactory = entityFactory;
        }

        /// <summary>
        /// Gets or sets the entity factory.
        /// </summary>
        /// <value>
        /// The entity factory.
        /// </value>
        public IEntityFactory EntityFactory { get; set; }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(Sitecore.Commerce.Pipelines.ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentCondition(args.Request is GetPartiesRequest, "args.Request", "args.Request is GetPartiesRequest");
            Assert.ArgumentCondition(args.Result is GetPartiesResult, "args.Result", "args.Result is GetPartiesResult");

            var request = (GetPartiesRequest)args.Request;
            var result = (GetPartiesResult)args.Result;
            Assert.ArgumentNotNull(request.CommerceCustomer, "request.CommerceCustomer");

            List<Party> partyList = new List<Party>();

            Profile customerProfile = null;
            var response = this.GetCommerceUserProfile(request.CommerceCustomer.ExternalId, ref customerProfile);
            if (!response.Success)
            {
                result.Success = false;
                response.SystemMessages.ToList().ForEach(m => result.SystemMessages.Add(m));
                return;
            }

            string preferredAddress = customerProfile["GeneralInfo.preferred_address"].Value as string;

            var profileValue = customerProfile["GeneralInfo.address_list"].Value as object[];
            if (profileValue != null)
            {
                var e = profileValue.Select(i => i.ToString());
                ProfilePropertyListCollection<string> addresIdsList = new ProfilePropertyListCollection<string>(e);
                if (addresIdsList != null)
                {
                    foreach (string addressId in addresIdsList)
                    {
                        Profile commerceAddress = null;
                        response = this.GetCommerceAddressProfile(addressId, ref commerceAddress);
                        if (!response.Success)
                        {
                            result.Success = false;
                            response.SystemMessages.ToList().ForEach(m => result.SystemMessages.Add(m));
                            return;
                        }

                        var newParty = this.EntityFactory.Create<CommerceParty>("Party");
                        var requestTorequestToEntity = new TranslateCommerceAddressProfileToEntityRequest(commerceAddress, newParty);
                        PipelineUtility.RunCommerceConnectPipeline<TranslateCommerceAddressProfileToEntityRequest, CommerceResult>("translate.commerceAddressProfileToEntity", requestTorequestToEntity);

                        if (!string.IsNullOrWhiteSpace(preferredAddress) && preferredAddress.Equals(newParty.ExternalId, System.StringComparison.OrdinalIgnoreCase))
                        {
                            newParty.IsPrimary = true;
                        }

                        var address = requestTorequestToEntity.DestinationParty;

                        partyList.Add(address);
                    }
                }
            }

            result.Parties = partyList.AsReadOnly();
        }
    }

}