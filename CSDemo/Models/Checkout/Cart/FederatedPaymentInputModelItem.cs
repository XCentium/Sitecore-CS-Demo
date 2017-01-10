using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Checkout.Cart
{
    public class FederatedPaymentInputModelItem
    {
        /// <summary>
        /// Gets or sets the payment method identifier.
        /// </summary>
        /// <value>
        /// The payment method identifier.
        /// </value>
        public string PaymentMethodID { get; set; }

        /// <summary>
        /// Gets or sets the card token.
        /// </summary>
        /// <value>
        /// The card token.
        /// </value>
        [Required]
        public string CardToken { get; set; }

        /// <summary>
        /// Gets or sets the card payment accept card prefix.
        /// </summary>
        /// <value>
        /// The card payment accept card prefix.
        /// </value>
        [Required]
        public string CardPaymentAcceptCardPrefix { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [Required]
        public decimal Amount { get; set; }
    }
}