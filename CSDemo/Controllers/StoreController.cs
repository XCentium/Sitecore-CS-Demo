using CSDemo.Models.Product;
using CSDemo.Models.Store;
using Glass.Mapper.Sc;
using Sitecore.Analytics;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System.Linq;
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


        private Item GetDatasourceItem()
        {
            var datasourceId = RenderingContext.Current.Rendering.DataSource;
            return Sitecore.Data.ID.IsID(datasourceId) ? Sitecore.Context.Database.GetItem(datasourceId) : null;
        }

        private LocationInformation GetUserLocation() {
            double? lat = null, lon = null;
            if (!(Tracker.Current == null ||
                    Tracker.Current.Interaction == null ||
                    Tracker.Current.Interaction.GeoData == null ||
                    Tracker.Current.Interaction.GeoData.Latitude == null ||
                    Tracker.Current.Interaction.GeoData.Longitude == null ||
                    Tracker.Current.Interaction.GeoData.Latitude == 0 ||
                    Tracker.Current.Interaction.GeoData.Longitude == 0)) 
               {
                   lat = Tracker.Current.Interaction.GeoData.Latitude;
                   lon = Tracker.Current.Interaction.GeoData.Longitude;

                   Sitecore.Diagnostics.Log.Info($"GetUserLocation() - Lat={lat}, Long={lon}", this);
            }
            else
            {
                Sitecore.Diagnostics.Log.Error("GetUserLocation() - Tracker is null.", this);
            }

            var userLocation = new LocationInformation
            {
                Latitude = lat,
                Longitude = lon
            };

            return userLocation;
        }

        public bool IsLocationValid(LocationInformation location) {
            var isValid = false;

            if ( (location.Latitude != null && location.Latitude != 0) &&
                 (location.Longitude != null && location.Longitude != 0)  )
            {
                isValid = true;
            }

            return isValid;
        }

        public ActionResult ClosestLocations()
        {
            var storeFolderItem = GetDatasourceItem();
            if (storeFolderItem == null) { return null; }
            var stores = storeFolderItem.Children.Select(c => c.GlassCast<Store>());
 
            var userLocation = GetUserLocation();
            const int numberOfStroresToShow = 4; // MG: Create a jira issue to make this a setting
            if (!IsLocationValid(userLocation))
            {
                if (stores.Count() > numberOfStroresToShow)
                {
                    stores = stores.Take(numberOfStroresToShow);
                }
                return View(stores);
            }

            var distances = Store.SortByProximity(userLocation, stores);

            if (distances == null || !distances.Any())
            {
                if (stores.Count() > numberOfStroresToShow)
                {
                    stores = stores.Take(numberOfStroresToShow);
                }
                return View(stores);
            }

            if (distances.Count() > numberOfStroresToShow)
            { 
                distances = distances.Take(numberOfStroresToShow);
            }
            return View(distances);
        }

        public ActionResult ProductInstockLocations()
        {
            var productId = WebUtil.GetUrlName(0).Replace(Constants.Common.Space, Constants.Common.Dash);
            if (productId.IsEmptyOrNull())
            {
                return null;
            }

            var model = ProductHelper.GetProductByNameAndCategory(productId, string.Empty);

            return View(model.Stores);
        }

    }
}