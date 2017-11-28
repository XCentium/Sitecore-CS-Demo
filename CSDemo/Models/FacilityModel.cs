using System.Collections.Generic;
using CSDemo.Models.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models
{
    [SitecoreType(AutoMap = true)]
    public class FacilityModel
    {
        [SitecoreField("Product Category Blacklist")]
        public virtual IEnumerable<Category> BlackListedProductCategories { get; set; }

		[SitecoreField("Address Line 1")]
		public virtual string AddressLine1 { get; set; }

		[SitecoreField("Address Line 2")]
		public virtual string AddressLine2 { get; set; }

		[SitecoreField("City")]
		public virtual string City { get; set; }

		[SitecoreField("State")]
		public virtual string State { get; set; }

		[SitecoreField("Postal Code")]
		public virtual string PostalCode { get; set; }
		
		[SitecoreField("Country")]
		public virtual string Country { get; set; }

		[SitecoreField("ExternalID")]
		public virtual string ExteernalId { get; set; }

	}
}