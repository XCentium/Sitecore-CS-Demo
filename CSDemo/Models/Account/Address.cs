using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class Address
    {
        public string AddressName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Id { get; set; }
        public string PartyId { get; set; }

        public bool IsMain { get; set; }
    }
}