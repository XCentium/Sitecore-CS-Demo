#region

using System.Collections.Generic;
using CSDemo.Models.CatalogGenerated;
using Sitecore.Mvc.Presentation;

#endregion

namespace CSDemo.Models.Product
{
    public class CategoryListingViewModel : RenderingModel
    {
        public IEnumerable<GeneralCategory> Categories { get; set; }

        public CategoryListingViewModel()
        {
        }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
        }
    }
}