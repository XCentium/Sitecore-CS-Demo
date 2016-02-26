using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;

namespace CSDemo.Models.Marquee
{
    public class CarouselViewModel : RenderingModel
    {
        IEnumerable<CarouselItem> Carouselitems { get; set; }
    }
}