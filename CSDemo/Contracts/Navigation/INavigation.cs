#region

using XCore.Framework.ItemMapper;

#endregion

namespace CSDemo.Contracts.Navigation
{
    public partial interface INavigation : ISitecoreItem
    {
        #region Properties

        string Name { get; set; }

        #endregion
    }
}