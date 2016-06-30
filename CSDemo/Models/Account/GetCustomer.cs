//using Sitecore.Commerce.Data.Customers;
//using Sitecore.Commerce.Pipelines;
//using Sitecore.Commerce.Pipelines.Customers.Common;

using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Diagnostics;

//using Sitecore.Commerce.Services.Customers;
//using Sitecore.Diagnostics;

namespace CSDemo.Models.Account
{
    public class GetCustomer : CustomerPipelineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomer"/> class.
        /// </summary>
        /// <param name="customerRepository">The user repository.</param>
        //public GetCustomer(ICustomerRepository customerRepository)
        //    : base(customerRepository)
        //{
        //}


        /// <summary>
        /// Executes the business logic of the GetCustomersFromSitecore processor.
        /// </summary>
        /// <param name="args">The arguments to run against.</param>
        public override void Process(ServicePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            var request = (GetCustomerRequest)args.Request;
            var result = (GetCustomerResult)args.Result;

            if (!string.IsNullOrEmpty(request.ExternalId))
            {
                var commerceProfile = UserObject.Get(request.ExternalId);

                result.CommerceCustomer = commerceProfile;
            }
        }
    }

}