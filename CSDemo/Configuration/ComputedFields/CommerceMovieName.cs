using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CSDemo.Configuration.ComputedFields
{
    public class CommerceMovieName : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            // item is currently being indexed
            Item item = indexable as SitecoreIndexableItem;

            if (item == null || !item.TemplateName.Equals("MovieVariant")) return null;

            var movieItem = item?.Parent;
            return movieItem != null ? movieItem.DisplayName : string.Empty;
        }
    }
}