#region

using Sitecore.Commerce.Services;
using Sitecore.Commerce.Services.Prices;

#endregion

namespace CSDemo.Services
{
    public class PricingServiceProvider : ServiceProvider
    {
        /// <summary>
        /// Gets the cart total.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns Cart totals</returns>
        public virtual Sitecore.Commerce.Services.Prices.GetCartTotalResult GetCartTotal(
            Sitecore.Commerce.Services.Prices.GetCartTotalRequest request)
        {
            return
                this
                    .RunPipeline
                    <Sitecore.Commerce.Services.Prices.GetCartTotalRequest,
                        Sitecore.Commerce.Services.Prices.GetCartTotalResult>("commerce.prices.getCartTotal", request);
        }

        /// <summary>
        /// Gets the product bulk prices.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns product bulk prices.</returns>
        public virtual GetProductBulkPricesResult GetProductBulkPrices(GetProductBulkPricesRequest request)
        {
            return
                this.RunPipeline<GetProductBulkPricesRequest, GetProductBulkPricesResult>(
                    "commerce.prices.getProductBulkPrices", request);
        }

        /// <summary>
        /// Gets the product prices.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns product prices.</returns>
        public virtual GetProductPricesResult GetProductPrices(GetProductPricesRequest request)
        {
            return this.RunPipeline<GetProductPricesRequest, GetProductPricesResult>(
                "commerce.prices.getProductPrices", request);
        }
    }
}