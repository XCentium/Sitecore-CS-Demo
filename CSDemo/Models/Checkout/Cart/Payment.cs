namespace CSDemo.Models.Checkout.Cart
{
    public class Payment
    {
        public string Token { get; set; }
        public string CardPrefix { get; set; }
        public Address BillingAddress { get; set; }

    }
}