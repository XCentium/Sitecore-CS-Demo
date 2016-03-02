#region

using CSDemo.Contracts.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

#endregion

namespace CSDemo.Models.Page
{
    [SitecoreType]
    public class Metadata : IMetadata
    {
        #region Members

        public const string MetaDescriptionFieldName = "Meta Description";

        public const string PageTitleFieldName = "Page Title";

        #endregion

        #region Properties

        [SitecoreField(MetaDescriptionFieldName)]
        public virtual string MetaDescription { get; set; }

        [SitecoreField(PageTitleFieldName)]
        public virtual string PageTitle { get; set; }

        #endregion
    }
}