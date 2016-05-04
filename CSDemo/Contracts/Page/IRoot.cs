
using Sitecore.Data.Items;

namespace CSDemo.Contracts.Page
{
    public interface IRoot
    {
        #region Properties

        #region Properties
        string Title { get; set; }
        string Email { get; set; }
        string PreTitle { get; set; }
        string Tagline { get; set; }
        string PhoneNumber { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        Item Catalog { get; set; }
        #endregion

        #endregion
    }
}
