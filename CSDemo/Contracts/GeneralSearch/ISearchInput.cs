namespace CSDemo.Contracts.GeneralSearch
{
    public interface ISearchInput
    {
        string Query { get; set; }
        string RedirectUrl { get; set; }
    }
}