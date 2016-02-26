using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using XCore.Framework;
using CSDemo.Models;
using CSDemo.Models.CatalogGenerated;

namespace CSDemo.Models.Product
{
    public class CategoryListingViewModel: RenderingModel
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