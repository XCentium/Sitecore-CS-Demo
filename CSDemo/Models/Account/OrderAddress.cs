using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class OrderAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }


        public string PartyId { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string ZipPostalCode { get; set; }
        public string CountryCode { get; set; }
        public string EveningPhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
    }
}