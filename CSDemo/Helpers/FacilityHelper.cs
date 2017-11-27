using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSDemo.Models;
using CSDemo.Models.InmateSearch;
using CSDemo.Models.Product;
using KeefePOC.Models;
using Sitecore.Diagnostics;

namespace CSDemo.Helpers
{
    public class FacilityHelper
    {
        public static List<Category> GetProgramProductCateogoryBlacklist()
        {
            var blacklist = new List<Category>();

            try
            {
                var facilityId = GetSelectedFacilityId();

                if (string.IsNullOrWhiteSpace(facilityId))
                    return blacklist;

                var facilityItem = Sitecore.Context.Database.GetItem(facilityId);
                var facility = GlassHelper.Cast<FacilityModel>(facilityItem);

                if (facility == null) return blacklist;

                blacklist = facility.BlackListedProductCategories.ToList();
            }
            catch (Exception e)
            {
                Log.Error($"FacilityHelper.GetProgramProductCateogoryBlacklist(), Error = {e.Message}", e);
            }

            return blacklist;
        }

        private static string GetSelectedFacilityId()
        {
            return GetSelectedFacility()?.Id;
        }

        private static Facility GetSelectedFacility()
        {
            return HttpContext.Current.Session["SELECTED_FACILITY"] as KeefePOC.Models.Facility;
        }

        public static void SaveSelectedFacility(Facility modelSelectedFacility)
        {
            HttpContext.Current.Session["SELECTED_FACILITY"] = modelSelectedFacility;
        }
    }
}