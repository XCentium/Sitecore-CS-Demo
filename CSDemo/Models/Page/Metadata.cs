#region

using CSDemo.Contracts.Page;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

#endregion

namespace CSDemo.Models.Page
{
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Metadata : SitecoreItem, ISitecoreItem, IMetadata
    {
        #region Members

        public const string SitecoreItemTemplateId = "{DC84F551-D6AD-46B4-80B0-2AB72BB0A419}";

        public const string MetaDescriptionFieldId = "{FBEFDCF0-C9DE-473A-AAB7-E043D2E335BA}";

        public const string MetaDescriptionFieldName = "Meta Description";

        public const string PageTitleFieldId = "{4D027FFA-64CF-4502-8C87-F5195FF18D76}";

        public const string PageTitleFieldName = "Page Title";

        #endregion

        #region Properties

        [SitecoreItemField(MetaDescriptionFieldId)]
        public virtual string MetaDescription { get; set; }

        [SitecoreItemField(PageTitleFieldId)]
        public virtual string PageTitle { get; set; }

        #endregion
    }
}