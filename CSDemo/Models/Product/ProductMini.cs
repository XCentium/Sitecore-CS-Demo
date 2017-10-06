
namespace CSDemo.Models.Product
{
    public class ProductMini
    {
        public string Id { get; set; }
        public string CatalogName { get; set; }
        public string CatalogId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Guid { get; set; }
        public string ImageSrc { get; set; }
        public string CategoryName { get; set; }
        public string VariantId { get; set; }
        public string Url { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsOnSale { get; set; }
        public decimal Rating { get; set; }
    }
}