using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Services;

namespace CSDemo.Services
{
    public class GetProductPricesResult : ServiceProviderResult
    {
        private readonly IDictionary<string, Dictionary<string, Price>> prices = new Dictionary<string, Dictionary<string, Price>>();

        /// <summary>
        /// Gets the prices by productId and price type.
        /// </summary>       
        public IDictionary<string, Dictionary<string, Price>> Prices
        {
            get
            {
                return this.prices;
            }
        }
    }
}