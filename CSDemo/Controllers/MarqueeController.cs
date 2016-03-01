#region

using System.Collections.Generic;
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
            var model = GetCarouselSlides();
            return View(model);
        }

        #endregion

        #region Private Helpers

        private IEnumerable<CarouselItem> GetCarouselSlides()
        {
            var items = new List<CarouselItem>();
            var datasource = RenderingContext.Current.Rendering.DataSource;
            if (datasource.IsEmptyOrNull()) return items;
            var parentItem = _context.Database.GetItem(datasource);
            if (parentItem == null) return items;
            var slides = parentItem.Children.OrderBy(i => i.Appearance.Sortorder).ToList();
            if (!slides.Any()) return items;
            items.AddRange(slides.Select(t => t.GlassCast<CarouselItem>()));
            return items;
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