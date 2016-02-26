#region

using CSDemo.Contracts.GeneralSearch;

#endregion

namespace CSDemo.Models.GeneralSearch
{
    public class SearchInput : ISearchInput
    {
        public string Query { get; set; }
        public string RedirectUrl { get; set; }
    }
}