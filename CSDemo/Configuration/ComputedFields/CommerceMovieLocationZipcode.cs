using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace CSDemo.Configuration.ComputedFields
{
    public class CommerceMovieLocationZipcode : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            //item is currently being indexed
            Item item = indexable as SitecoreIndexableItem;

            if (item == null || !item.TemplateName.Equals("MovieVariant")) return null;

            var locationId = item.Fields[Constants.Commerce.MovieLocationId]?.ToString();

            var masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
            var locationItem = string.IsNullOrWhiteSpace(locationId) ? null : masterDb.GetItem(new ID(locationId));

            return locationItem != null ? locationItem["Zipcode"] : string.Empty;
        }
    }
}