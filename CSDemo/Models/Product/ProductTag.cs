using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class ProductTag : IProductTag
    {
        [SitecoreField(TagConstants.TagName)]
        public virtual string TagName { get; set; }

        public struct TagConstants
        {
            public const string TagName = "Tag Name";
        }
    }
}