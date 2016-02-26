#region

using CSDemo.Contracts;
using XCore.Framework.ItemMapper;
using XCore.Framework.ItemMapper.Configuration.Attributes;

#endregion

namespace CSDemo.Models
{
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Components : SitecoreItem, ISitecoreItem, IComponents
    {
        #region Members

        public const string SitecoreItemTemplateId = "{24216566-BD19-4EB9-B421-1CB7A97060E7}";

        #endregion
    }
}