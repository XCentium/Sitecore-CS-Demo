using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;

namespace CSDemo.Models.Marquee
{
    public class CarouselViewModel : RenderingModel
    {
        public IEnumerable<CarouselItem> CarouselSlides { get; set; }
    }
}