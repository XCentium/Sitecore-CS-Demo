using CSDemo.Models.Store;
using Glass.Mapper.Sc;
using Sitecore.Analytics;
using Sitecore.Analytics.Lookups;
using Sitecore.Diagnostics;
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
            Log.Info("getting closest locations", this);
            var storeFolderItem = RenderingContext.Current.Rendering.Item;
            if (storeFolderItem == null) return null;
            var stores = storeFolderItem.Children.Select(c => c.GlassCast<Store>());
            const int numberOfStroresToShow = 4;
            Log.Info("Geo Client IP is " + Tracker.Current.Interaction.Ip, this);
            if (Tracker.Current == null || Tracker.Current.Interaction == null || Tracker.Current.Interaction.GeoData == null 
                || Tracker.Current.Interaction.GeoData.Latitude== 0 || Tracker.Current.Interaction.GeoData.Longitude==0)
            {
                Log.Info("can't get client coors", this);
                if (stores.Count() > numberOfStroresToShow)
                    stores = stores.Take(4);
                return View(stores);
            }

            Log.Info("coors found", this);
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