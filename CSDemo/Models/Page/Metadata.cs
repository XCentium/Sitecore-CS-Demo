#region

using CSDemo.Contracts.Page;
using Glass.Mapper.Sc.Configuration.Attributes;

#endregion

namespace CSDemo.Models.Page
{
    [SitecoreType]
    public class Metadata : IMetadata
    {
        #region Members

        public const string MetaDescriptionFieldName = Constants.Page.MetaDescription;

        public const string PageTitleFieldName = Constants.Page.PageTitle;

        #endregion

        #region Properties

        [SitecoreField(MetaDescriptionFieldName)]
        public virtual string MetaDescription { get; set; }

        [SitecoreField(PageTitleFieldName)]
        public virtual string PageTitle { get; set; }

        #endregion
    }
}