namespace CSDemo.Models.Checkout.Cart
{
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string ZipPostalCode { get; set; }
        public string ExternalId { get; internal set; }
		public string InmateId { get; set; }
	}
}