using Sitecore.Mvc.Presentation;

namespace CSDemo.Models.Product
{
    public class CategoriesViewModel : RenderingModel
    {
        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ItemId { get; set; }

        public CategoriesViewModel()
        {
            Image = string.Empty;
            Url = string.Empty;
            Name = string.Empty;
            ItemId = string.Empty;
        }

    }
}