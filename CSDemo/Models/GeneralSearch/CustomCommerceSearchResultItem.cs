using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.ContentSearch;

namespace CSDemo.Models.GeneralSearch
{
    [SitecoreType(AutoMap = true)]
    public class CustomCommerceSearchResultItem : CommerceProductSearchResultItem
    {
        [IndexField("commerceproducttags")]
        public List<string> ProductTags { get; set; }

        [IndexField("male")]
        public bool RestrictionMale { get; set; }

        [IndexField("female")]
        public bool RestrictionFemale { get; set; }

        [IndexField("sugarfree")]
        public bool RestrictionSugarFree { get; set; }

        [IndexField("kosher")]
        public bool RestrictionKosher { get; set; }

        [IndexField("glutenfree")]
        public bool RestrictionGlutenFree { get; set; }
    }
}
