using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Checkout.Cart
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Carts;
    using Sitecore.Commerce.Entities.Shipping;

    /// <summary>
    /// Defines the GetShippingMethodsRequest class.
    /// </summary>
    public class GetShippingMethodsRequest : CommerceGetShippingMethodsRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetShippingMethodsRequest"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        public GetShippingMethodsRequest(string language) : this(language, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetShippingMethodsRequest"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="shippingOption">The shipping option.</param>
        /// <param name="party">The party.</param>
        public GetShippingMethodsRequest(string language, ShippingOption shippingOption, Party party = null)
            : base(language, shippingOption, party)
        {
        }

        /// <summary>
        /// Gets or sets the cart.
        /// </summary>
        /// <value>
        /// The cart.
        /// </value>
        public Cart Cart { get; set; }

        /// <summary>
        /// Gets or sets the lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<CartLine> Lines { get; set; }
    }
}