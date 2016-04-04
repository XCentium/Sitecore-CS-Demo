using System.Collections.Generic;
using System.Linq;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Search.ComputedFields;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace CSDemo.Configuration
{
    public class CommerceProductTags : BaseCommerceComputedField
    {
        private static readonly IEnumerable<ID> ValidTemplatesInternal = new List<ID>
        {
            CommerceConstants.KnownTemplateIds.CommerceProductTemplate,
            CommerceConstants.KnownTemplateIds.CommerceProductVariantTemplate
        }.AsReadOnly();

        protected override IEnumerable<ID> ValidTemplates => ValidTemplatesInternal;

        public override object ComputeValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = GetValidatedItem(indexable);
            MultilistField productsField = item?.Fields[Constants.Commerce.ProductTagsFieldId];
            if (productsField?.GetItems() == null) return null;
            var items = productsField.GetItems();
            return (from n in items
                select n.Fields[Constants.Commerce.ProductTagNameFieldId]
                into field
                where field != null && field.HasValue
                select field.Value).ToList();
        }
    }
}