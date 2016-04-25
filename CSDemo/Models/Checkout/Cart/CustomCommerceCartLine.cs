#region

using System;
using System.Collections.Generic;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Data;

#endregion

namespace CSDemo.Models.Checkout.Cart
{
    public class CustomCommerceCartLine : CommerceCartLine
    {
        private List<String> _images;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommerceCartLine"/> class.
        /// </summary>
        public CustomCommerceCartLine()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommerceCartLine"/> class.
        /// </summary>
        /// <param name="productCatalog">The product catalog.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="variantId">The variant identifier.</param>
        /// <param name="quantity">The quantity.</param>
        public CustomCommerceCartLine(string productCatalog, string productId, string variantId, uint quantity)
            : base(productCatalog, productId, variantId, quantity)
        {
        }

        private string _sitecoreID;

        public string SitecoreID
        {
            get
            {
                if (!string.IsNullOrEmpty(this._sitecoreID))
                {
                    return this._sitecoreID;
                }


                return (!ID.IsNullOrEmpty(Product.SitecoreProductItemId) ? this.Product.SitecoreProductItemId.ToString(): null);
            }
        }


        public List<String> Images
        {
            get
            {
                if (this._images != null && this._images.Count > 0)
                {
                    return this._images;
                }

                this._images = new List<String>();

                var field = this.Properties["_product_Images"] as string;

                if (!string.IsNullOrEmpty(field))
                {
                    var imageIds = field.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var id in imageIds)
                    {
                        var productImageItem = Sitecore.Context.Database.GetItem(id);

                        var imageUrl = "/~/media/" + productImageItem.ID.ToShortID() + ".ashx";
                        this._images.Add(imageUrl);
                    }
                }

                return this._images;
            }
        }
    }
}