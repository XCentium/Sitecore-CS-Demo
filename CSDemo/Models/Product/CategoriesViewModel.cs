
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Product
{
    public class CategoriesViewModel : RenderingModel
    {
        public string Image { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string ItemID { get; set; }

        public CategoriesViewModel()
        {
            this.Image = string.Empty;
            this.Url = string.Empty;
            this.Name = string.Empty;
            this.ItemID = string.Empty;
        }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
        }
    }
}