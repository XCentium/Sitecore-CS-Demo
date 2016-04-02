using CSDemo.Contracts.Tagging;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace CSDemo.Models.Tagging
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