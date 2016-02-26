using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Helpers
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
                Item ProductImageItem = Sitecore.Context.Database.GetItem(ProductImages[0]);
                ProductImage = "/~/media/" + ProductImageItem.ID.ToShortID() + ".ashx";
            }


            return ProductImage;
        }





    }
}