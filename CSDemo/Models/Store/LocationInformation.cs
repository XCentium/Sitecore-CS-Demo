using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Store
{
    public class LocationInformation
    {
        public virtual Guid Id { get; set; }
        public virtual double? Latitude { get; set; }
        public virtual double? Longitude { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Country { get; set; }
    }
}