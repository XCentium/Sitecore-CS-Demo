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


        public Item GetDatasourceItem()
        {
            var datasourceId = RenderingContext.Current.Rendering.DataSource;
            return Sitecore.Data.ID.IsID(datasourceId) ? Sitecore.Context.Database.GetItem(datasourceId) : null;
        }
        public ActionResult ClosestLocations()
        {
            var storeFolderItem = GetDatasourceItem();
            if (storeFolderItem == null) { return null; }
            var stores = storeFolderItem.Children.Select(c => c.GlassCast<Store>());
            const int numberOfStroresToShow = 4; // MG: Create a jira issue to make this a setting
            if (Tracker.Current == null || 
                Tracker.Current.Interaction == null || 
                Tracker.Current.Interaction.GeoData == null ||
                Tracker.Current.Interaction.GeoData.Latitude == null ||
                Tracker.Current.Interaction.GeoData.Longitude == null ||
                Tracker.Current.Interaction.GeoData.Latitude == 0 ||
                Tracker.Current.Interaction.GeoData.Longitude == 0       )
            {
                if (stores.Count() > numberOfStroresToShow)
                {
                    stores = stores.Take(numberOfStroresToShow);
                }
                return View(stores);
            }

            var origin = new LocationInformation
            {
                Latitude = Tracker.Current.Interaction.GeoData.Latitude,
                Longitude = Tracker.Current.Interaction.GeoData.Longitude
            };
            var distances = Store.SortByProximity(origin, stores);

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

            Product model = ProductHelper.GetProductByNameAndCategory(productId, string.Empty);

            return View(model.Stores);
        }

    }
}