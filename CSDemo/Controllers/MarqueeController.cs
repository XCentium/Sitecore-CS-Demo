#region

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CSDemo.Models.Marquee;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.DynamicSerialization;
using Sitecore.Mvc.Extensions;
using Sitecore.Resources.Media;
using XCore.Framework;
using CSDemo.Contracts.Product;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Core;
using Sitecore.Analytics.Tracking;
using Sitecore.Data;
using Sitecore.Globalization;
using System;
using CSDemo.Models.Product;

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

        private static IEnumerable<IProduct> GetRecentlyViewedProducts()
        {
            var products = new List<IProduct>();
            ITracker tracker = Tracker.Current;
            if (tracker == null) return products;
            if (tracker.Contact == null) return products;

            if (tracker.Interaction == null || tracker.Interaction.Pages == null || tracker.Interaction.Pages.Length == 0)
                return products;

            foreach (var page in tracker.Interaction.Pages)
            {
                if (page.Item == null) continue;
                var itemId = new ID(page.Item.Id);
                var language = Language.Parse(page.Item.Language);
                var item = Sitecore.Context.Database.GetItem(itemId, language);
                if (item == null) continue;

                if (item.TemplateID == new ID(Constants.Pages.ProductDetailPageId))
                {
                    //Get product here from the details page
                    //  products.Add(item.GlassCast<Product>());
                }

                var maxNumberOfProductsToShow = 10;
                if (products.Count > maxNumberOfProductsToShow) break;
            }
            return products;
        }

        private IEnumerable<CarouselItem> GetCarouselSlides()
        {
            var items = new List<CarouselItem>();
            try
            {
                var datasource = RenderingContext.Current.Rendering.DataSource;
                if (datasource.IsEmptyOrNull()) return items;
                var parentItem = _context.Database.GetItem(datasource);
                if (parentItem == null) return items;
                var slides = parentItem.Children.OrderBy(i => i.Appearance.Sortorder).ToList();
                if (!slides.Any()) return items;
                items.AddRange(slides.Select(t => t.GlassCast<CarouselItem>()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
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