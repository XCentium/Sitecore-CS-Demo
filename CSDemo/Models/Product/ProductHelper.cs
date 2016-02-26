#region

using Sitecore.Data.Items;

#endregion

namespace CSDemo.Models.Product
{
    public static class ProductHelper
    {
        public static string GetFirstImageFromProductItem(Item product)
        {
            var ProductImageIds = product["Images"];
            var ProductImage = string.Empty;
            if (!string.IsNullOrEmpty(ProductImageIds))
            {
                var ProductImages = ProductImageIds.Split('|');
                var ProductImageItem = Sitecore.Context.Database.GetItem(ProductImages[0]);
                ProductImage = "/~/media/" + ProductImageItem.ID.ToShortID() + ".ashx";
            }


            return ProductImage;
        }
    }
}