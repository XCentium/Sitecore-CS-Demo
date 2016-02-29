#region

using System.Web.Mvc;
using CSDemo.Models.Marquee;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Linq;
using Sitecore.DynamicSerialization;
using Sitecore.Mvc.Extensions;
using Sitecore.Resources.Media;
using XCore.Framework;

#endregion

namespace CSDemo.Controllers
{
    public class MarqueeController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreContext _context;

        #endregion

        #region Methods

        public ActionResult Carousel()
        {
            var model = GetCarouselViewModel();
            return View(model);
        }

        #endregion

        #region Private Helpers

        private CarouselViewModel GetCarouselViewModel()
        {
            CarouselViewModel model = new CarouselViewModel();
            var datasource = RenderingContext.Current.Rendering.DataSource;
            if (datasource.IsEmptyOrNull()) return model;
            var parentItem = _context.Database.GetItem(datasource);
            if (parentItem == null) return model;
            var slides = parentItem.Children.OrderBy(i => i.Appearance.Sortorder).CreateAs<CarouselItem>().ToList();
            if (!slides.Any()) return model;
            model.DisplayOn = true;
            foreach (var slide in slides)
            {
                var image = slide.Image;
                if (image != null)
                {
                    var url = MediaManager.GetMediaUrl(image.MediaItem);
                    if (!url.IsEmptyOrNull())
                    {
                        slide.Url = url;
                    }
                }
                // Image and Content are necessary to display a slide, hide if not available
                if (!slide.Url.IsEmptyOrNull() && !slide.Content.IsEmptyOrNull())
                    slide.CanShowSlide = true;
            }
            model.CarouselSlides = slides;
            return model;
        }

        #endregion

        #region Constructors

        public MarqueeController(ISitecoreContext context)
        {
            _context = context;
        }

        public MarqueeController() : this(new SitecoreContext())
        {
        }

        #endregion
    }
}