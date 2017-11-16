#region
		
using System.Collections.Generic;
using System.Linq;
using CSDemo.Models.Product;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Search.ComputedFields;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
 
#endregion

namespace CSDemo.Configuration
{
    public class CommerceProductGroups : BaseCommerceComputedField
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

            if (item == null)
            {
                return new List<string>();
            }

            var groups = ProductHelper.GetProductGroups(item).ToList();

            return groups;
        }
    }
}