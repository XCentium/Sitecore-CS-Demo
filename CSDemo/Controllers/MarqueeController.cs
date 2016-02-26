using System.Web.Mvc;
using CSDemo.Models.Marquee;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

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

        #region Private Helpers

        private CarouselViewModel GetCarouselViewModel()
        {
            CarouselViewModel model = new CarouselViewModel();
            var ds = RenderingContext.Current.Rendering.DataSource;
            // TBD:
            return model;
        }


        #endregion

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