#region

using System.Collections.Generic;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Services;

#endregion

namespace CSDemo.Services
{
    public class GetProductPricesResult : ServiceProviderResult
    {
        /// <summary>
        /// Gets the prices by productId and price type.
        /// </summary>       
        public IDictionary<string, Dictionary<string, Price>> Prices { get; } =
            new Dictionary<string, Dictionary<string, Price>>();
    }
}