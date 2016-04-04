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
    }
}
