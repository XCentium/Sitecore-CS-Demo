#region

using XCore.Framework.ItemMapper;

#endregion

namespace CSDemo.Contracts.Page
{
    public partial interface IMetadata : ISitecoreItem
    {
        #region Properties

        string MetaDescription { get; set; }

        string PageTitle { get; set; }

        #endregion
    }
}