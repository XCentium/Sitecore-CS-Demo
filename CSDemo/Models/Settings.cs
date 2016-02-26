#region

using CSDemo.Contracts;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

#endregion

namespace CSDemo.Models
{
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Settings : SitecoreItem, ISitecoreItem, ISettings
    {
        #region Members

        public const string SitecoreItemTemplateId = "{A21B890E-C798-469C-8727-0F2C525787F3}";

        #endregion
    }
}