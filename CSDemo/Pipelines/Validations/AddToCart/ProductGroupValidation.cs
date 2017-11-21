using System.Linq;
using CSDemo.Models.Checkout.Cart;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Diagnostics;

namespace CSDemo.Pipelines.Validations.AddToCart
{
    // This validation should really be done in SAP and return error codes
    public class ProductGroupValidation : PipelineProcessor<ServicePipelineArgs>
    {
        public override void Process(ServicePipelineArgs args)
        {
            var request = (CartLinesRequest)args.Request;

            //do not execute if previous processor already failed
            if (!args.Result.Success)
            {
                return;
            }

            var line = (request.Lines.Select(l => l).FirstOrDefault());
            var productId = line?.Product?.ProductId;
            var qty = line?.Quantity == null ? 0  : int.Parse(line.Quantity.ToString());

            Assert.ArgumentCondition(!string.IsNullOrWhiteSpace(productId), "productId", "productId is null");
            Assert.ArgumentCondition(qty  > 0, "qty", "qty is 0");

            if (string.IsNullOrWhiteSpace(productId))
            {
                args.Result.Success = false;
                args.Result.SystemMessages.Add(new SystemMessage("Product Id is null or empty."));
            }

            var validation = new CartHelper().ValidatePurchase(productId, qty);

            args.Result.Success = validation.IsValidForPurchase;

            if (!string.IsNullOrWhiteSpace(validation.Message))
            {
                args.Result.SystemMessages.Add(new SystemMessage(validation.Message));
            }
        }

    }
}
