using CSDemo.Models.Store;
using Glass.Mapper.Sc;
using Sitecore.Analytics;
using Sitecore.Analytics.Lookups;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
    public class StoreController : Controller
    {
        #region Fields

        private readonly ISitecoreContext _context;

        #endregion

        #region Constructors

        public StoreController(ISitecoreContext context)
        {
            _context = context;
        }

        public StoreController()
            : this(new SitecoreContext())
        {
        }

        #endregion

        public ActionResult ClosestLocations()
        {
            var storeFolderItem = RenderingContext.Current.Rendering.Item;
            if (storeFolderItem == null) return null;
            var stores = storeFolderItem.Children.Select(c => c.GlassCast<Store>());
            const int numberOfStroresToShow = 4;
            if (Tracker.Current == null || Tracker.Current.Interaction == null || Tracker.Current.Interaction.GeoData == null || Tracker.Current.Interaction.GeoData.Latitude== null || Tracker.Current.Interaction.GeoData.Longitude==null)
            {
                if (stores.Count() > numberOfStroresToShow)
                    stores = stores.Take(4);
                return View(stores);
            }


            var origin = new LocationInformation
            {
                Latitude = Tracker.Current.Interaction.GeoData.Latitude,
                Longitude = Tracker.Current.Interaction.GeoData.Longitude
            };
            var distances = Store.SortByProximity(origin, stores);
            if (distances == null || !distances.Any()) return null;
            
            if (distances.Count() > numberOfStroresToShow)
                distances = distances.Take(4);
            return View(distances);
        }


    }
}