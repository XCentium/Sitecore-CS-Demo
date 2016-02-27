#region

using System.Web.Mvc;
using CSDemo.Models.Marquee;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Linq;
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
            var parentItem = _context.Database.GetItem(datasource);
            if(parentItem != null)
            {
                var slides = parentItem.Children.OrderBy(i => i.Appearance.Sortorder).CreateAs<CarouselItem>().ToList();
                if(slides != null && slides.Any())
                {
                    model.CarouselSlides = slides;
                }
            }
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