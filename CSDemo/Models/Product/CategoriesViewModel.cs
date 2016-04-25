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
            this.Image = string.Empty;
            this.Url = string.Empty;
            this.Name = string.Empty;
            this.ItemId = string.Empty;
        }

    }
}